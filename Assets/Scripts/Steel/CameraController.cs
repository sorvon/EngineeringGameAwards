using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;




public class CameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool isDraging = false;
    bool isHover = false;
    Vector3 lastMousePos;
    Camera cam;
    float distance = 10;
    float zoomSpeed = 10f;
    Vector3 lookAroundPos = Vector3.zero;
    private void Awake()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        if (isDraging)
        {
            Vector3 newMousePos = Input.mousePosition;
            Vector3 delta = newMousePos - lastMousePos;
            cam.transform.position += CalcDragLength(cam, delta, distance);
            lastMousePos = newMousePos;
        }
        if (isHover)
        {
            float scrollWheelValue = Input.GetAxis("Mouse ScrollWheel");
            if (!Mathf.Approximately(scrollWheelValue, 0f))
            {
                // scrollWheelValue����������Ϊ+ ����Ϊ-
                // ͬʱ�������ϣ�����������������£���Զ���
                //distance -= scrollWheelValue * zoomSpeed;
                //distance = Mathf.Clamp(distance, minDistance, maxDistance);
                cam.orthographicSize -= scrollWheelValue * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2f, 20f);
                //cam.transform.position = lookAroundPos - cam.transform.rotation * (Vector3.forward * distance);

                // �������ı����ͬʱ�޸�fov��orthographicSize�Լ�ͶӰ���󣬱�֤����/͸����ͼ�л�Ч��
                //CalculateProjMatrix();
            }
        }
    }


    private Vector3 CalcDragLength(Camera camera, Vector2 mouseDelta, float distance)
    {
        float rectHeight = -1;
        float rectWidth = -1;
        if (camera.orthographic)
        {
            rectHeight = 2 * camera.orthographicSize;
            //rectWidth = rectHeight / camera.aspect;
        }
        else
        {
            rectHeight = 2 * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }
        rectWidth = Screen.width * rectHeight / Screen.height;
        Vector3 moveDir = -rectWidth / Screen.width * mouseDelta.x * camera.transform.right - rectHeight / Screen.height * mouseDelta.y * camera.transform.up;

        return moveDir;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        isDraging = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isDraging = true;
        lastMousePos = Input.mousePosition;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}
