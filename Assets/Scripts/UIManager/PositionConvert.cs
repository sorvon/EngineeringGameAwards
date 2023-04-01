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
        // 将obj1的世界坐标转换为屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(obj1.transform.position);

        // 将屏幕坐标转换为ui坐标
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(obj2, screenPos, null, out localPos);

        // 将ui坐标赋值给obj2的anchoredPosition3D属性
        obj2.anchoredPosition3D = localPos;
    }
}
