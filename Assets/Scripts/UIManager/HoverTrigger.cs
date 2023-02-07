using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject popupObject;
    public Vector3 mouseOffset;
    // Update is called once per frame
    void Update()
    {
        if (popupObject.activeSelf)
        {
            popupObject.transform.position = Input.mousePosition + mouseOffset;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (popupObject != null)
        {
            popupObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (popupObject != null)
        {
            popupObject.SetActive(false);
        }
    }


}
