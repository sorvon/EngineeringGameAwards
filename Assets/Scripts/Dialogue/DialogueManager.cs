using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying; //{ get; private set; }

    private static DialogueManager instance;
    [SerializeField] private TextMeshProUGUI displayNameText;
  /*  [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;*/

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    //private const string AUDIO_TAG = "audio";

    public SpriteRenderer speakerLeft;
    public SpriteRenderer speakerRight;
    private SpriteRenderer speakerNow;
    private Sprite crtNow;
    public Sprite crtA;
    public Sprite crtB;
    public Sprite crtC;
    public RectTransform speakerFrame;
    public RectTransform positionLeft;
    public RectTransform positionRight;
    public GameObject trigger;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        //dialogueChoices.SetActive(false);
        // get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        Debug.Log(dialogueIsPlaying);
        // return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            dialoguePanel.SetActive(false);
            return;
        }

        // handle continuing to the next line in the dialogue when submit is pressed
        // NOTE: The 'currentStory.currentChoiecs.Count == 0' part was to fix a bug after the Youtube video was made
        if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        Debug.Log("开始对话");
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        //yield return new WaitForSeconds(0.2f);
        Debug.Log("退出对话");
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        trigger.SetActive(false);
        return;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            dialogueText.text = currentStory.Continue();
            // display choices, if any, for this dialogue line
            DisplayChoices();
            HandleTags(currentStory.currentTags);
            Debug.Log("继续对话");
        }
        else
        {
            
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            dialogueText.text = "";
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        // NOTE: The below two lines were added to fix a bug after the Youtube video was made
        //InputManager.GetInstance().RegisterSubmitPressed(); // this is specific to my InputManager script
        Input.GetKeyDown(KeyCode.Space);
        ContinueStory();
    }

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    Debug.Log(tagValue);
                    displayNameText.text = tagValue;
                    CharacterConvert(tagValue);
                    break;
              /*  case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;*/
                case LAYOUT_TAG:
                    Debug.Log(tagValue);
                    if (tagValue == "left")
                    {
                        speakerLeft.color = Color.white;
                        speakerRight.color = Color.gray;
                        speakerNow = speakerLeft;
                        speakerFrame.position = positionLeft.position;
                        speakerLeft.sprite = crtNow;
                    }
                    else
                    {
                        speakerLeft.color = Color.gray;
                        speakerRight.color = Color.white;
                        speakerNow = speakerRight;
                        speakerFrame.position = positionRight.position;
                        speakerRight.sprite = crtNow;
                    }
                    //layoutAnimator.Play(tagValue);
                    break;
/*                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;*/
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
    void CharacterConvert(string tagValue)
    {
        switch (tagValue)
        {
            case "A":
                crtNow = crtA;
                break;
            case "B":
                crtNow = crtB;
                break;
            case "C":
                crtNow = crtC;
                break;
            default:
                Debug.LogWarning("tag:speaker出现问题");
                break;
        }
    }
}
