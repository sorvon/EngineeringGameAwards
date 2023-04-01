using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class PositionConvert : MonoBehaviour
{
    public GameObject obj1;
    public RectTransform obj2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void convert()
    {
        // ��obj1����������ת��Ϊ��Ļ����
        Vector3 screenPos = Camera.main.WorldToScreenPoint(obj1.transform.position);

        // ����Ļ����ת��Ϊui����
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(obj2, screenPos, null, out localPos);

        // ��ui���긳ֵ��obj2��anchoredPosition3D����
        obj2.anchoredPosition3D = localPos;
    }
}
