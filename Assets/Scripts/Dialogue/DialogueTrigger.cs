using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour, IDataPersistence
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private int digLevel = 1;
    [SerializeField] public TextAsset[] successDialogText;
    public bool successDialog = false;
    GameData gameData;

    //private bool toEnterDialogue;
    /*  void Awake()
      {
          toEnterDialogue = false;
      }*/

    // Update is called once per frame
    //void Update()
    //{
    //    if (toEnterDialogue && !DialogueManager.GetInstance().dialogueIsPlaying)
    //    {

    //    }
    //}
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("successDialogue"))
        {
            PlayerPrefs.SetInt("successDialogue", -1);
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying) return;
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Base":
                if (gameData.baseDialog)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                    gameData.baseDialog = false;
                    PlayerPrefs.SetInt("successDialogue", -1);
                }
                //锻造成功后的对话
                if (PlayerPrefs.GetInt("successDialogue") >= 0)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(successDialogText[PlayerPrefs.GetInt("successDialogue")]);
                    PlayerPrefs.SetInt("successDialogue", -1);
                }
                
                break;
            case "Steel":
                if (gameData.steelDialog)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                    gameData.steelDialog = false;
                }
                break;
            case "SteelRolling":
                if (gameData.steelRollingDialog)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                    gameData.steelRollingDialog = false;
                }
                break;
            case "Quench DYH":
                if (gameData.quenchDialog)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                    gameData.quenchDialog = false;
                }
                break;
            case "Dig":
                if (gameData.digLevel == digLevel && gameData.digDialogs[digLevel-1])
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                    gameData.digDialogs[digLevel - 1] = false;
                }
                break;
            default:
                break;
        }
        DataPersistenceManager.instance.SaveGame();
    }

    public void EnterDialogue()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
        
        //toEnterDialogue = true;
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        this.gameData = gameData;
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        gameData = this.gameData;
    }
}
