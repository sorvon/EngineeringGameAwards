using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI lastTimeText;
    [SerializeField] private TextMeshProUGUI durationTimeText;
    [SerializeField] private ConfirmDialog confirmDialog;
    [SerializeField] private Animator sceneTransition;
    public bool HasData { get; private set; }
    SaveSlot()
    {
        HasData = false;
    }
    public void SetData(GameData gameData)
    {
        if (gameData == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            HasData = false;
        }
        else
        {
            HasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            lastTimeText.text = gameData.lastTime;
            durationTimeText.text = "游戏时长: " + gameData.GetFormatDurationTime();    
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void OnSlotClicked()
    {
        DataPersistenceManager.instance.ChangeSelectedProfileId(profileId);
        if (!HasData)
        {
            DataPersistenceManager.instance.NewGame();
        }
        else
        {
            DataPersistenceManager.instance.LoadGame();
        }
        DataPersistenceManager.instance.SaveGame();
        var gameData = DataPersistenceManager.instance.GetCurrentGameData();
        var swordImages = sceneTransition.gameObject.GetComponentsInChildren<Image>(true);
        bool[] swords = gameData.swords;
        for (int i = 0; i < swords.Length; i++)
        {
            if (swords[i])
            {
                swordImages[i+2].enabled = false;
                swordImages[i+10].enabled = true;
            }
            else
            {
                swordImages[i + 2].enabled = true;
                swordImages[i + 10].enabled = false;
            }
        }
        LoadScene(profileId);
        sceneTransition.SetTrigger("Start");
        StartCoroutine(LoadScene("Base"));
    }

    public void OnDeleteSlotClicked()
    {
        if (!HasData)
        {
            return;
        }
        confirmDialog.ActivateMenu("确认删除吗？",
            () =>
            {
                DataPersistenceManager.instance.DeleteData(profileId);
                SetData(null);
            },
            () => { });
    }

    IEnumerator LoadScene(string name)
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(name);
    }
}
