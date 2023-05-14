using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HandbookManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI describeText;
    [SerializeField] Image swordImage;
    [SerializeField] string[] names;
    [SerializeField] string[] descriptions;
    [SerializeField] Sprite[] sprites;

    public void SetSword(int value)
    {
        nameText.text = names[value];
        describeText.text = descriptions[value];
        swordImage.sprite = sprites[value];
    }
}
