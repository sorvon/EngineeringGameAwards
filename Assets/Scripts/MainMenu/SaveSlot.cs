using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public bool hasData { get; private set; }
    SaveSlot()
    {
        hasData = false;
    }
    public void SetData(GameData gameData)
    {
        if (gameData == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            hasData = false;
        }
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            lastTimeText.text = gameData.lastTime;
            durationTimeText.text = "”Œœ∑ ±≥§: " + gameData.GetFormatDurationTime();    
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void OnSlotClicked()
    {
        DataPersistenceManager.instance.ChangeSelectedProfileId(profileId);
        if (!hasData)
        {
            DataPersistenceManager.instance.NewGame();
        }
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("Base");
    }

    public void OnDeleteSlotClicked()
    {
        DataPersistenceManager.instance.DeleteData(profileId);
        SetData(null);
    }
}
