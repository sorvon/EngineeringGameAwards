using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class ConfirmDialog : MonoBehaviour 
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public void ActivateMenu(string text, UnityAction confirmAction, UnityAction cancelAction)
    {
        gameObject.SetActive(true);
        displayText.text = text;
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            confirmAction();   
        });
        cancelButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            cancelAction();
        });
    }
}
