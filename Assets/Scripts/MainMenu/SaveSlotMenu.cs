using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotMenu : MonoBehaviour
{
    [Header("Menu Navi")]
    [SerializeField] private MainMenu mainMenu;
    private SaveSlot[] saveSlots;
    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }
    public void OnBackClicked()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ActivateMenu()
    {
        gameObject.SetActive(true);
        var profilesGameData = DataPersistenceManager.instance.GatAllProfilesGameData();
        foreach (var saveSlot in saveSlots)
        {
            GameData gameData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out gameData);
            saveSlot.SetData(gameData);
        }
    }
}
