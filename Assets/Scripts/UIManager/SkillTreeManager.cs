using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class SkillTreeManager : MonoBehaviour, IDataPersistence
{
    [Header("Skill Level")]
    [SerializeField] int ProcessLevel = 1;
    [SerializeField] Image[] skillImages;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Button[] levelButton;
    [SerializeField] TextMeshProUGUI ancientNumText;

    [Header("Skill tree config")]
    [SerializeField] Vector3 cameraPos;
    [SerializeField] float orthogrphicSize = 0.95f;
    [SerializeField] private Vector3 oldCameraPos;
    [SerializeField] private float oldOrthogrphicSize;
    [SerializeField] private TextAsset inkJSON;

    [Header("Debug")]
    [SerializeField] int ancientNum = 0;
    [SerializeField] bool skillDialog;
    private void Awake()
    {
        oldCameraPos = new Vector3(0, 0, -10);
        oldOrthogrphicSize = 5;
    }
    public void OnSkillTreeClicked()
    {
        var camera = Camera.main;
        oldCameraPos = camera.transform.position;
        oldOrthogrphicSize = camera.orthographicSize;
        camera.transform.DOMove(cameraPos, 1);
        camera.DOOrthoSize(orthogrphicSize, 1).OnComplete(() =>
        {
            gameObject.SetActive(true);
            if (skillDialog)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                skillDialog = false;
                DataPersistenceManager.instance.SaveGame();
            }
        });
    }
    public void OnBackClicked()
    {
        gameObject.SetActive(false);
        var camera = Camera.main;
        camera.transform.DOMove(oldCameraPos, 1);
        camera.DOOrthoSize(oldOrthogrphicSize, 1);
    }

    public void SetSkillLevel(int value)
    {
        if (ancientNum>0)
        {
            ancientNum--;
            ancientNumText.text = Convert.ToString(ancientNum);
            ProcessLevel = value;
            UpdateUI();
            DataPersistenceManager.instance.SaveGame();
        }
        
    }

    void UpdateUI()
    {
        if (ProcessLevel >= 2)
        {
            levelButton[0].interactable = false;
        }
        if (ProcessLevel >= 3)
        {
            levelButton[1].interactable = false;
        }
        for (int i = 0; i < 9; i++)
        {
            if (i < ProcessLevel * 3)
            {
                skillImages[i].sprite = sprites[1];
            }
            else
            {
                skillImages[i].sprite = sprites[0];
            }
        }
        ancientNumText.text = Convert.ToString(ancientNum);
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        ancientNum = gameData.ancientNum;
        ProcessLevel = gameData.ProcessLevel;
        skillDialog = gameData.skillDialog;
        UpdateUI();
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        gameData.ancientNum = ancientNum;
        gameData.ProcessLevel = ProcessLevel;
        gameData.skillDialog = skillDialog;
    }
}
