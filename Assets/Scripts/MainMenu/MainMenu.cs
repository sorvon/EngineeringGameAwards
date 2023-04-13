using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navi")]
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        saveSlotMenu.ActivateMenu();
        gameObject.SetActive(false);
        //DataPersistenceManager.instance.NewGame();
        //SceneManager.LoadScene("SampleScene");
    }
    public void OnContinueGameClicked()
    {

    }
}
