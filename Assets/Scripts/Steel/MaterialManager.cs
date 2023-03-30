using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialManager : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IDragHandler,
    IBeginDragHandler, IEndDragHandler
{
    public Vector3 direction;
    public CanvasGroup canvasGroup;
    private GameObject player;
    private LineRenderer lineRenderer;
    private Vector3 thisPos;
    private Canvas canvas;
    
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        canvas = GetComponent<Canvas>();
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
        lineRenderer.enabled = true;
        var player_pos = player.transform.position;
        lineRenderer.SetPosition(0, player_pos);
        lineRenderer.SetPosition(1, player_pos+direction);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        lineRenderer.enabled = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        lineRenderer.enabled = true;
        transform.position = Input.mousePosition;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        canvas.sortingOrder = 10;
        canvasGroup.blocksRaycasts = false;
        thisPos = transform.position;
        
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        canvas.sortingOrder = 1;
        canvasGroup.blocksRaycasts = true;
        lineRenderer.enabled = false;
        transform.position = thisPos;
    }
}
