using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HandbookManager : MonoBehaviour, IDataPersistence
{
    [Header("Config")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI describeText;
    [SerializeField] Image swordImage;
    [SerializeField] string[] names;
    [SerializeField] string[] descriptions;
    [SerializeField] Sprite[] sprites;
    bool[] swords;
    public void SetSword(int value)
    {
        nameText.text = names[value];
        swordImage.sprite = sprites[value];
        if (swords[value])
        {
            describeText.text = descriptions[value];
            swordImage.color = new Color(255, 255, 255);
        }
        else
        {
            describeText.text = "Î´½âËø";
            swordImage.color = new Color(0, 0, 0);
        }
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        swords = gameData.swords;
    }

    void IDataPersistence.SaveData(GameData gameData)
    {

    }
}
