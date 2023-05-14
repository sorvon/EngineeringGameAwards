using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class HandbookManager : MonoBehaviour, IDataPersistence
{
    [Header("Config")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI describeText;
    [SerializeField] TextMeshProUGUI describeText2;
    [SerializeField] Image swordImage;
    [SerializeField] Image swordBackImage;
    [SerializeField] string[] names;
    [SerializeField]
    TextAsset[] inks;
    [SerializeField] string[] descriptions;
    [SerializeField] string[] descriptions2;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Sprite enableBackSprite;
    [SerializeField] Sprite disableBackSprite;
    int currentIndex = 0;
    bool[] swords;

    private void Start()
    {
        SetSword(currentIndex);
    }
    public void SetSword(int value)
    {
        currentIndex = value;
        nameText.text = names[value];
        swordImage.sprite = sprites[value];
        describeText.text = descriptions[value];
        if (swords[value])
        {
            describeText2.text = Regex.Unescape(descriptions2[value]);
            swordImage.color = new Color(255, 255, 255);
            swordBackImage.sprite = enableBackSprite;
        }
        else
        {
            describeText2.text = "Î´½âËø";
            swordImage.color = new Color(0, 0, 0);
            swordBackImage.sprite = disableBackSprite;
        }
    }

    public void DialogTrigger()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inks[currentIndex]);
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        swords = gameData.swords;
    }

    void IDataPersistence.SaveData(GameData gameData)
    {

    }
}
