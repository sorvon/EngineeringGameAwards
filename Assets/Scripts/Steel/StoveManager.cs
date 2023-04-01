using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
public class StoveManager : MonoBehaviour, IDropHandler
{
    public float speed = 2;
    private static float rotate;
    private LineRenderer lineRenderer;
    private bool hasMatrial;
    private GameObject player;
    
    private void Awake()
    {
        rotate = 0;
        lineRenderer = GetComponent<LineRenderer>();
        hasMatrial = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        var newLineRenderer = eventData.pointerDrag.GetComponent<LineRenderer>();
        lineRenderer.material = newLineRenderer.material;
        lineRenderer.startWidth = newLineRenderer.startWidth;
        lineRenderer.endWidth = newLineRenderer.endWidth;
        lineRenderer.positionCount = newLineRenderer.positionCount;
        Vector3[] postions = new Vector3[newLineRenderer.positionCount];
        newLineRenderer.GetPositions(postions);
        lineRenderer.SetPositions(postions);
        hasMatrial = true;
        print(postions[0]);
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
                    if (lineRenderer.positionCount >= 2)
                    {
                        if((lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1)).magnitude <= lineRenderer.startWidth)
                        {
                            Vector3[] newPostions = new Vector3[lineRenderer.positionCount - 1];
                            for (int j = 0; j < newPostions.Length; j++)
                            {
                                newPostions[j] = postions[postions.Length - newPostions.Length + j];
                            }
                            lineRenderer.SetPositions(newPostions);
                            lineRenderer.positionCount = newPostions.Length;
                            lineRenderer.SetPosition(0, player.transform.position);
                        }
                    }
                    

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
    }
}
