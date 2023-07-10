using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverUIShowName : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoImage;
    public string AnnotationText;

    //偏移值
    [SerializeField] float offsetX = 0;
    [SerializeField] float offsety = 0;

    void Start()
    {
    /*    offsetX=infoImage.GetComponent<RectTransform>().rect.width/2;
        offsety = infoImage.GetComponent<RectTransform>().rect.height / 2;*/
    }
    /// <summary>
    /// 控制UI的位置
    /// </summary>
    void Update()
    {
        if (infoImage.activeSelf)
        {
            infoImage.transform.position = Input.mousePosition + new Vector3(offsetX, offsety, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoImage != null)
        {
            infoImage.SetActive(true);
            infoImage.GetComponentInChildren<Text>().text = AnnotationText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoImage != null)
        {
            infoImage.SetActive(false);
        }
    }
}
