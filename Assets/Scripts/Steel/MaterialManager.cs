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
    [Header("Config")]
    public float width = 5;
    public float height = 1.5f;
    public string type;
    [Header("audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mineDragBegin;
    [SerializeField] AudioClip mineDragEnd;
    [SerializeField] AudioClip mineDragDrop;
    private CanvasGroup canvasGroup;
    private GameObject player;
    private LineRenderer lineRenderer;
    private Vector3 thisPos;
    private Canvas canvas;
    [Header("Debug")]
    [SerializeField] public Vector3[] rawPoints;
    private int pointNum;
    static public bool lockHover;
    static public bool isDropped;
    
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
        pointNum = 100;
        switch (type.ToLower())
        {
            case "sin":
                rawPoints = new Vector3[pointNum + 1];
                for (int i = 0; i < rawPoints.Length; i++)
                {
                    float x = (float)i / pointNum * width;
                    float y = Mathf.Sin(x / width * 2 * Mathf.PI) * height;
                    rawPoints[i] = new Vector3(x, y, 0);
                }
                break;
            case "cos":
                rawPoints = new Vector3[pointNum + 1];
                for (int i = 0; i < rawPoints.Length; i++)
                {
                    float x = (float)i / pointNum * width;
                    float y = Mathf.Cos(x / width * 2 * Mathf.PI) * height - height;
                    rawPoints[i] = new Vector3(x, y, 0);
                }
                break;
            case "xsin":
                rawPoints = new Vector3[pointNum + 1];
                for (int i = 0; i < rawPoints.Length; i++)
                {
                    float x = (float)i / pointNum * width;
                    float y = x * Mathf.Sin(x / width * 2 * Mathf.PI) * height;
                    rawPoints[i] = new Vector3(x, y, 0);
                }
                break;
            case "-xsin":
                rawPoints = new Vector3[pointNum + 1];
                for (int i = 0; i < rawPoints.Length; i++)
                {
                    float x = (float)i / pointNum * width;
                    float y = -x * Mathf.Sin(x / width * 2 * Mathf.PI) * height;
                    rawPoints[i] = new Vector3(x, y, 0);
                }
                break;
            default:
                pointNum = lineRenderer.positionCount;
                rawPoints = new Vector3[pointNum];
                lineRenderer.GetPositions(rawPoints);
                Vector3 diff = player.transform.position - rawPoints[0];
                for (int i = 0; i < rawPoints.Length; i++)
                {
                    rawPoints[i] = rawPoints[i] - rawPoints[0];
                }
                break;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (lockHover) return;
        lineRenderer.enabled = true;
        Vector3[] points = new Vector3[Mathf.CeilToInt(rawPoints.Length * StoveManager.oxygen / 4.0f)];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Quaternion.AngleAxis(StoveManager.rotate, Vector3.forward) * rawPoints[i];
            points[i] = points[i] + player.transform.position;
        }
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
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
        isDropped = false;
        if(lockHover) 
        {
            eventData.pointerDrag = null;
            return;
        }
        canvas.sortingOrder = 10;
        canvasGroup.blocksRaycasts = false;
        thisPos = transform.position;
        audioSource.PlayOneShot(mineDragBegin);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        
        lockHover = false;
        canvas.sortingOrder = 1;
        canvasGroup.blocksRaycasts = true;
        lineRenderer.enabled = false;
        transform.position = thisPos;
        //print(eventData.)
        if (isDropped)
        {
            audioSource.PlayOneShot(mineDragDrop);
        }
        else
        {
            audioSource.PlayOneShot(mineDragEnd);
        }
        
    }
}
