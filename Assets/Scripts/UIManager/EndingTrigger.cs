using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndingTrigger : MonoBehaviour, IDataPersistence
{
    static public EndingTrigger instance;
    GameData gameData;
    void IDataPersistence.LoadData(GameData gameData)
    {
        this.gameData = gameData;
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        gameData = this.gameData;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        var swords = gameData.swords;
        bool isEnd = true;
        foreach (var sword in swords)
        {
            if (!sword)
            {
                isEnd = false;
            }
        }
        //isEnd = true;
        if (isEnd && PlayerPrefs.GetInt("successDialogue") >= 0 && PlayerPrefs.GetInt("successDialogue") == 7)
        {
            if (TryGetComponent<Animator>(out var animator))
            {
                animator.enabled = true;
                var text = GetComponentInChildren<TextMeshProUGUI>(true);
                if (text != null)
                {
                    text.outlineWidth = 0.158f;
                }
            }
        }
    }
}
