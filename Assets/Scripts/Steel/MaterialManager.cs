using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class MaterialManager : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IDragHandler,
    IBeginDragHandler, IEndDragHandler
{
    public float width = 5;
    public float height = 1.5f;
    public string type;
    private CanvasGroup canvasGroup;
    private GameObject player;
    private LineRenderer lineRenderer;
    private Vector3 thisPos;
    private Canvas canvas;
    static public bool lockHover;
    
    // Start is called before the first frame update
    private void Awake()
    {
        lockHover = false;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (lockHover) return;
        lineRenderer.enabled = true;
        var player_pos = player.transform.position;
        int pointNum = 100;
        switch (type.ToLower())
        {
            case "sin":
                Vector3[] points = new Vector3[pointNum+1];
                for (int i = 0; i <= pointNum; i++)
                {
                    float x = (float)i / pointNum * width;
                    float y = Mathf.Sin(x / width * 2 * Mathf.PI) * height;
                    points[i] = player_pos + new Vector3(x, y, 0);
                }
                lineRenderer.positionCount = pointNum+1;
                lineRenderer.SetPositions(points);
                break;
            case "cos":
                points = new Vector3[pointNum+1];
                for (int i = 0; i <= pointNum; i++)
                {
                    float x = (float)i / pointNum * width;
                    float y = Mathf.Cos(x / width * 2 * Mathf.PI) * height - height;
                    points[i] = player_pos + new Vector3(x, y, 0);
                }
                lineRenderer.positionCount = pointNum+1;
                lineRenderer.SetPositions(points);
                break;
            default:
                pointNum = lineRenderer.positionCount;
                points = new Vector3[pointNum];
                lineRenderer.GetPositions(points);
                Vector3 diff = player_pos - points[0];
                for (int i = 0; i < pointNum; i++)
                {
                    points[i] = diff + points[i];
                }
                lineRenderer.SetPositions(points);
                break;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        lineRenderer.enabled = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        lockHover = true;
        lineRenderer.enabled = true;
        transform.position = Input.mousePosition;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(lockHover) 
        {
            eventData.pointerDrag = null;
            return;
        }
        canvas.sortingOrder = 10;
        canvasGroup.blocksRaycasts = false;
        thisPos = transform.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        lockHover = false;
        canvas.sortingOrder = 1;
        canvasGroup.blocksRaycasts = true;
        lineRenderer.enabled = false;
        transform.position = thisPos;
    }
}
