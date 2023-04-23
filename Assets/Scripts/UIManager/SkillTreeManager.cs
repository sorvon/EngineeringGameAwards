using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SkillTreeManager : MonoBehaviour
{
    [Header("Skill Level")]
    public int skill_1 = 0;
    [SerializeField] GameObject skill_ui_1;
    public int skill_2 = 0;
    [SerializeField] GameObject skill_ui_2;
    public int skill_3 = 0;
    [SerializeField] GameObject skill_ui_3;

    [Header("Skill tree config")]
    [SerializeField] Vector3 cameraPos;
    [SerializeField] float orthogrphicSize = 0.95f;
    [SerializeField] private Vector3 oldCameraPos;
    [SerializeField] private float oldOrthogrphicSize;

    public void OnSkillTreeClicked()
    {
        var camera = Camera.main;
        oldCameraPos = camera.transform.position;
        oldOrthogrphicSize = camera.orthographicSize;
        camera.transform.DOMove(cameraPos, 1);
        camera.DOOrthoSize(orthogrphicSize, 1).OnComplete(() =>
        {
            gameObject.SetActive(true);
        });
    }
    public void OnBackClicked()
    {
        gameObject.SetActive(false);
        var camera = Camera.main;
        camera.transform.DOMove(oldCameraPos, 1);
        camera.DOOrthoSize(oldOrthogrphicSize, 1);
    }
}
