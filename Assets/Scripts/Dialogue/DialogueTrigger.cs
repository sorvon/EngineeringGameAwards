using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool toEnterDialogue;
    void Awake()
    {
        toEnterDialogue = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (toEnterDialogue && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
        
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        
        }
    }

    public void EnterDialogue()
    {
        toEnterDialogue = true;
    }
}
