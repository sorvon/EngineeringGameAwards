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
    [SerializeField] private ConfirmDialog confirmDialog;
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
            durationTimeText.text = "��Ϸʱ��: " + gameData.GetFormatDurationTime();    
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
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("Base");
    }

    public void OnDeleteSlotClicked()
    {
        if (!HasData)
        {
            return;
        }
        confirmDialog.ActivateMenu("ȷ��ɾ����",
            () =>
            {
                DataPersistenceManager.instance.DeleteData(profileId);
                SetData(null);
            },
            () => { });
        
    }
}
