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
            //var clamp_x = Mathf.Clamp(cam.transform.position.x, -50f + cam.orthographicSize* 1302.8f / 681.47f, 50f - cam.orthographicSize * 1302.8f / 681.47f);
            var clamp_x = Mathf.Clamp(cam.transform.position.x, -36.7f + cam.orthographicSize, 45f - cam.orthographicSize );
            var clamp_y = Mathf.Clamp(cam.transform.position.y, -35f + cam.orthographicSize, 30f - cam.orthographicSize);
            cam.transform.position = new Vector3(clamp_x, clamp_y, cam.transform.position.z);
            lastMousePos = newMousePos;
        }
        if (isHover)
        {
            float scrollWheelValue = Input.GetAxis("Mouse ScrollWheel");
            if (!Mathf.Approximately(scrollWheelValue, 0f))
            {
                // scrollWheelValue：滚轮向上为+ 向下为-
                // 同时滚轮向上，拉近相机；滚轮向下，拉远相机
                //distance -= scrollWheelValue * zoomSpeed;
                //distance = Mathf.Clamp(distance, minDistance, maxDistance);
                cam.orthographicSize -= scrollWheelValue * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2f, 20f);
                var clamp_x = Mathf.Clamp(cam.transform.position.x, -36.7f + cam.orthographicSize, 45f - cam.orthographicSize);
                var clamp_y = Mathf.Clamp(cam.transform.position.y, -35f + cam.orthographicSize, 30f - cam.orthographicSize);
                cam.transform.position = new Vector3(clamp_x, clamp_y, cam.transform.position.z);
                //cam.transform.position = lookAroundPos - cam.transform.rotation * (Vector3.forward * distance);

                // 相机距离改变后需同时修改fov或orthographicSize以及投影矩阵，保证正交/透视视图切换效果
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
