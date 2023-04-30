using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using Cinemachine;

[RequireComponent(typeof(LineRenderer))]
public class StoveManager : MonoBehaviour, IDropHandler
{
    [Header("Config")]
    public float speed = 2;
    public TextMeshProUGUI oxygenUI;
    public static float rotate = 0;
    public static int oxygen = 2;
    [SerializeField] Button finishButton;
    [SerializeField] TargetManager targetManager;
    [SerializeField] GameObject failMenu;
    private LineRenderer lineRenderer;
    private bool hasMatrial;
    private GameObject player;
    private Vector3[] rawPoints;
    AudioSource audioSource;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        hasMatrial = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        oxygenUI.text = Convert.ToString(oxygen);
    }


    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        var incomeLineRenderer = eventData.pointerDrag.GetComponent<LineRenderer>();
        MaterialManager materialManager = eventData.pointerDrag.GetComponent<MaterialManager>();
        lineRenderer.material = incomeLineRenderer.material;
        lineRenderer.startWidth = incomeLineRenderer.startWidth;
        lineRenderer.endWidth = incomeLineRenderer.endWidth;
        
        rawPoints = materialManager.rawPoints;
        Vector3[] points = new Vector3[Mathf.CeilToInt(rawPoints.Length * oxygen / 4.0f)];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Quaternion.AngleAxis(rotate, Vector3.forward) * rawPoints[i];
            points[i] = points[i] + player.transform.position;
        }
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        hasMatrial = true;
        MaterialManager.isDropped = true;
    }

    public void MoveTrigger()
    {
        if (hasMatrial)
        {
            audioSource.Play();
            MaterialManager.lockHover = true;
            
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
            Sequence sequence = DOTween.Sequence();
            Vector3[] postions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(postions);
            player.transform.position = postions[0];
            player.transform.DOPath(postions, speed).SetSpeedBased()
                .OnUpdate(() =>
                {
                    lineRenderer.SetPosition(0, player.transform.position);
                })
                .OnWaypointChange((index) =>
                {
                    if (index == 0)
                    {
                        return;
                    }
                    Vector3[] newPostions = new Vector3[lineRenderer.positionCount - 1];
                    for (int j = 0; j < newPostions.Length; j++)
                    {
                        newPostions[j] = postions[postions.Length - newPostions.Length + j];
                    }
                    lineRenderer.SetPositions(newPostions);
                    lineRenderer.positionCount = newPostions.Length;
                    lineRenderer.SetPosition(0, player.transform.position);
                })
                .OnComplete(() =>
                {
                    MaterialManager.lockHover = false;
                    audioSource.Pause();
                    Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                    finishButton.interactable = targetManager.CheckFinish();
                });
            hasMatrial = false;
        }
    }

    public void SetRotate(float value)
    {
        rotate = value;
        if (!hasMatrial) return;
        if (rawPoints == null) return;
        Vector3[] points = new Vector3[Mathf.CeilToInt(rawPoints.Length * oxygen / 4.0f)];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Quaternion.AngleAxis(rotate, Vector3.forward) * rawPoints[i];
            points[i] = points[i] + player.transform.position;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    public void TriggerRotate(Transform button)
    {
        rotate-=90;
        if (rotate == -360)
        {
            rotate = 0;
        }
        button.transform.DORotate(new Vector3(0, 0, rotate), 0.2f);
        //button.rotation = Quaternion.AngleAxis(rotate-90, Vector3.forward);
        if (!hasMatrial) return;
        if (rawPoints == null) return;
        
        //print(rotate);
        Vector3[] points = new Vector3[Mathf.CeilToInt(rawPoints.Length * oxygen / 4.0f)];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Quaternion.AngleAxis(rotate, Vector3.forward) * rawPoints[i];
            points[i] = points[i] + player.transform.position;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    public void OxygenChange(int value)
    {
        if (oxygen + value <= 4 && oxygen + value >=1)
        {
            oxygen += value;
            oxygenUI.text = Convert.ToString(oxygen);
            if (!hasMatrial) return;
            Vector3[] points = new Vector3[Mathf.CeilToInt(rawPoints.Length * oxygen / 4.0f)];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Quaternion.AngleAxis(rotate, Vector3.forward) * rawPoints[i];
                points[i] = points[i] + player.transform.position;
            }
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }
    }

    public void OnPlayerCollision()
    {
        hasMatrial = false;
        MaterialManager.lockHover = false;
        lineRenderer.positionCount = 0;
        audioSource.Stop();
        failMenu.SetActive(true);
    }
}
