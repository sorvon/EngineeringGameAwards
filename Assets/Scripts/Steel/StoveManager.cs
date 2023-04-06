using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class StoveManager : MonoBehaviour, IDropHandler
{
    public float speed = 2;
    public TextMeshProUGUI oxygenUI;
    public static float rotate = 0;
    public static int oxygen = 4;
    private LineRenderer lineRenderer;
    private bool hasMatrial;
    private GameObject player;
    private Vector3[] rawPoints;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hasMatrial = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        oxygenUI.text = Convert.ToString(oxygen);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void MoveTrigger()
    {
        if (hasMatrial)
        {
            MaterialManager.lockHover = true;
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
                });
            hasMatrial = false;
        }
    }

    public void SetRotate(float value)
    {
        rotate = value;
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

    public void OxygenChange(int value)
    {
        if (oxygen + value <= 4 && oxygen + value >=1)
        {
            oxygen += value;
            oxygenUI.text = Convert.ToString(oxygen);
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
}
