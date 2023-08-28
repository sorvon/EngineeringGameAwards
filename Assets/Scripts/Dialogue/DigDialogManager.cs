using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DigDialogManager : MonoBehaviour, IDataPersistence
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset baseDigInk;
    //private int digLevel = 1;
    private bool baseDigDialog;
    private bool isToDig = false;

    private void Update()
    {
        if (isToDig && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            SceneManager.LoadScene("Dig");
        }
    }
    public void ToDig()
    {
        if (baseDigDialog)
        {
            DialogueManager.GetInstance().EnterDialogueMode(baseDigInk);
            baseDigDialog = false;
            DataPersistenceManager.instance.SaveGame();
        }
        isToDig = true;
    }
    void IDataPersistence.LoadData(GameData gameData)
    {
        //digLevel = gameData.digLevel;
        baseDigDialog = gameData.baseDigDialog;
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        //gameData.digLevel = digLevel;
        gameData.baseDigDialog = baseDigDialog;
    }
}
