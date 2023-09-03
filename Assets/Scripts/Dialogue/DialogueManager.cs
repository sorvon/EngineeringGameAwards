using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Image speakerLeft;
    public Image speakerRight;
    private Image speakerNow;
    private Sprite crtNow;
    public Sprite portraitA1;
    public Sprite portraitA2;
    public Sprite portraitB;
    public Sprite portraitC;
    public Sprite none;
    public RectTransform speakerFrame;
    public RectTransform positionLeft;
    public RectTransform positionRight;
    public Sprite[] cgImages;
    public Image specialCG;

    private UnityEngine.Object tempAnnotator;
    private UnityEngine.Object tempAnnotator2;

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
        //Debug.Log(dialogueIsPlaying);
        // return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            dialoguePanel.SetActive(false);
            return;
        }

        // handle continuing to the next line in the dialogue when submit is pressed
        // NOTE: The 'currentStory.currentChoiecs.Count == 0' part was to fix a bug after the Youtube video was made
        if (currentStory.currentChoices.Count == 0 && (Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown (0)))
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
        specialCG.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        speakerLeft.enabled = false;
        speakerRight.enabled = false;
        //trigger.SetActive(false);
        return;
    }

    private void ContinueStory()
    {
        //
/*        dialogueText.gameObject.GetComponent<TMP_TextEventHandler>().annoWord1 = "";
        dialogueText.gameObject.GetComponent<TMP_TextEventHandler>().annoText1 = "";
        dialogueText.gameObject.GetComponent<TMP_TextEventHandler>().annoWord2 = "";
        dialogueText.gameObject.GetComponent<TMP_TextEventHandler>().annoText2 = "";
        dialogueText.gameObject.GetComponent<TMP_TextEventHandler>().annoWordNow = "";
        UnityEngine.Object.Destroy(tempAnnotator);
        UnityEngine.Object.Destroy(tempAnnotator2);
*/
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

        //StartCoroutine(SelectFirstChoice());
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
            /*if (splitTag.Length != 2)
            {
                if (splitTag.Length == 3 && splitTag[0].Trim()=="anno")
                {
                    *//*Debug.Log("添加注释" + tag);
                    GetComponent<TextAnnotator>().AddAnnotation(splitTag[1].Trim(), splitTag[2].Trim());*//*
                }
                else
                    Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }*/
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case "anno":
                    Debug.Log("添加注释" + tag + "  1:" + splitTag[1].Trim() + "  2:" + splitTag[2].Trim());

                    dialogueText.gameObject.GetComponent<TMP_TextEventHandler0>().annoWord1 = splitTag[1].Trim();
                    dialogueText.gameObject.GetComponent<TMP_TextEventHandler0>().annoText1 = splitTag[2].Trim();
                    //tempAnnotator = GetComponent<TextAnnotator>().AddAnnotation(splitTag[1].Trim(), splitTag[2].Trim());
                    break;
                case "anno2":
                    Debug.Log("添加注释" + tag + "  1:" + splitTag[1].Trim() + "  2:" + splitTag[2].Trim());
                    dialogueText.gameObject.GetComponent<TMP_TextEventHandler0>().annoWord2 = splitTag[1].Trim();
                    dialogueText.gameObject.GetComponent<TMP_TextEventHandler0>().annoText2 = splitTag[2].Trim();
                    //tempAnnotator2 = GetComponent<TextAnnotator>().AddAnnotation(splitTag[1].Trim(), splitTag[2].Trim());
                    break;

                case SPEAKER_TAG:
                    Debug.Log(tagValue);
                    displayNameText.text = tagValue;
                    if (tagValue == "旁白")
                    {
                        Debug.Log("旁白文字");
                        speakerFrame.gameObject.SetActive(false);
                        speakerLeft.enabled = false;
                        speakerRight.enabled = false;
                    }
                    else
                    {
                        //speakerLeft.enabled = true;
                        //speakerRight.enabled = true;
                        speakerFrame.gameObject.SetActive(true);
                    }
                    //CharacterConvert(tagValue);
                    break;
                case PORTRAIT_TAG:
                    CharacterConvert(tagValue);
                    break;
                case LAYOUT_TAG:
                    Debug.Log(tagValue);
                    if (tagValue == "left")
                    {
                        speakerRight.color = Color.gray;
                        speakerLeft.color = Color.white;
                        
                        //speakerNow = speakerLeft;
                        speakerFrame.position = positionLeft.position;
                        speakerLeft.sprite = crtNow;
                        if (speakerLeft.sprite) speakerLeft.enabled = true;
                        //if (speakerRight.sprite) speakerRight.enabled = true;
                    }
                    else
                    {  
                        speakerLeft.color = Color.gray;
                        speakerRight.color = Color.white;
                        //speakerNow = speakerRight;
                        speakerFrame.position = positionRight.position;
                        speakerRight.sprite = crtNow;
                        //if (speakerLeft.sprite) speakerLeft.enabled = true;
                        if (speakerRight.sprite) speakerRight.enabled = true;
                    }
                    //layoutAnimator.Play(tagValue);
                    break;
                /*                case AUDIO_TAG:
                                    SetCurrentAudioInfo(tagValue);
                                    break;*/
                case "CG":
                    specialCG.gameObject.SetActive(true);
                    specialCG.sprite = cgImages[int.Parse(tagValue)];
                    break;

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
            case "A1":
                crtNow = portraitA1;
                break;
            case "A2":
                crtNow = portraitA2;
                break;
            case "B":
                crtNow = portraitB;
                break;
            case "C":
                crtNow = portraitC;
                break;
            default:
                Debug.LogWarning("tag:protrait出现问题");
                break;
        }
    }
}
