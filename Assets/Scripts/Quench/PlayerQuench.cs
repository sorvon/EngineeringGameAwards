using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class PlayerQuench : MonoBehaviour, IDataPersistence
{
    [Header("��Ƶ")]
    public AudioSource scoreUpSound;
    public AudioSource scoreDownSound;
    public AudioSource usingScrollbarSound;
    public AudioSource quenchingSound;
    [Header("Config")]
    public float horizontalForce = 10;
    public float verticalForce = 1;
    public float verticalForceInterval = 1;
    public float destTolerance = 1;
    public Scrollbar[] scrollbarList;
    static double[] scrollbarValues;
    public Transform destination;
    public GameObject successPanel;
    
    public GameObject failurePanel;
    public GameObject failureReturnPanel;
    [SerializeField] Image steelImage;
    [SerializeField] Sprite[] steelSprites;
    public Text scoretext;
    public int punishmentvalue;
    public int rewardvalue;                   //����ӷַ�ֵ
    public TextMeshProUGUI statistics;        //���������Ȱ�װ����
    public TextMeshProUGUI adjusttimetext;    //ʣ���������
    [Header("�����������")]
    //�����������
    public TextMeshProUGUI scorechangeup;       
    public TextMeshProUGUI scorechangedown;
    public TextMeshProUGUI plus3;
    public TextMeshProUGUI timetext;

    [Header("�Ѷ�GameObject")]
    [SerializeField] GameObject[] levels = new GameObject[3];
    [Header("Debug")]
    [SerializeField] bool overrideDifficulty;
    [SerializeField] int defaultDifficulty = 1;
    [SerializeField] int defaultSteelIndex = 0;


    public int score;
    public static int adjusttime = 5;
    string scrollBase;
    public bool isTrigger;
    Rigidbody2D rb;
    Vector3 initPos;
    float timeCount;

    // Start is called before the first frame update
    void Awake()
    {
        score = 10;
        scrollBase = "scroll";
        isTrigger = false;
        initPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        //�̳�ǰһ���趨�Ĺ�������
        for (int i = 0; i < scrollbarList.Length; i++)
        {
            if (PlayerPrefs.HasKey(scrollBase + i))
            {
                scrollbarList[i].value = PlayerPrefs.GetFloat(scrollBase + i);
            }
        }
        adjusttimetext.text = "" + adjusttime;
        steelImage.sprite = steelSprites[PlayerPrefs.GetInt("steel_index", 0)];
        if (overrideDifficulty)
        {
            PlayerPrefs.SetInt("difficulty", defaultDifficulty);
        }
        int difficulty = PlayerPrefs.GetInt("difficulty", defaultDifficulty);
        score = 5 + 5 * difficulty;
        for (int i = 0; i < levels.Length; i++)
        {
            if (i + 1 == difficulty)
            {
                levels[i].SetActive(true);
            }
            else
            {
                levels[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (isTrigger)
        {
            timeCount += Time.deltaTime;
            timetext.text = "" + timeCount.ToString("0.0");
        }
        //�ɹ����
        /*if (Vector3.Distance(transform.position, destination.position) < destTolerance)
        {
            *//*isTrigger = false;
            successPanel.SetActive(true);
            rb.velocity = Vector2.zero;*//*
            successes();
        }*/
        //scorechangedown.rectTransform.position = transform.position;
    }

    private void FixedUpdate()
    {
        if (isTrigger)//���ݹ���λ��ʩ����Ӧ��С����
        {
            int index = (int)(timeCount / verticalForceInterval);
            if (index < scrollbarList.Length)
            {
                float froce = (scrollbarList[index].value - 0.5f) * 2 * verticalForce;
                rb.AddForce(new Vector2(0, froce));
            }
        }
    }


    public void Trigger()//��ʼ���
    {
        //rb.velocity = new Vector2(5, 0);
        if (adjusttime <= 0)
        {
            failureReturnPanel.SetActive(true);
        }
        if (adjusttime > 0 && !isTrigger)
        {
            for (int i = 0; i < scrollbarList.Length; i++)
            {
                PlayerPrefs.SetFloat(scrollBase + i, scrollbarList[i].value);
            }
            adjusttime--;
            adjusttimetext.text = "" + adjusttime;

            rb.AddForce(new Vector2(horizontalForce, 0), ForceMode2D.Impulse); 
            timeCount = 0;
            quenchingSound.Play(); 
            isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)//�����յ�
    {

        if (collision.collider.CompareTag("DangerEdge"))
        {
            print("��ʱ��" + timeCount + " ��");
            isTrigger = false;
            transform.position = initPos;
            rb.velocity = Vector2.zero;
            failurePanel.SetActive(true);
        }
        if (collision.collider.CompareTag("Reward1"))
        {
            score += 1;
            Successe();
            adjusttime++;
        }
        if (collision.collider.CompareTag("Reward2"))
        {
            score += 2;
            Successe();
            adjusttime++;
        }
        if (collision.collider.CompareTag("Reward3"))
        {
            score += 3;
            Successe();
            adjusttime++;
            //����+3
            plus3.rectTransform.position = transform.position;
            FlyTo(plus3);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)//�������ϰ�����
    {

        if (collision.CompareTag("Reward"))
        {
            scoreUpSound.Play();
            score += rewardvalue;
            collision.gameObject.SetActive(false);
            scorechangeup.gameObject.SetActive(true);
            scorechangeup.rectTransform.position = transform.position;
            FlyTo(scorechangeup);//����
            
        }
        if (collision.CompareTag("Punishment"))
        {
            scoreDownSound.Play();
            score -= punishmentvalue;
            collision.gameObject.SetActive(false);
            scorechangedown.gameObject.SetActive(true);
            scorechangedown.rectTransform.position = transform.position;
            FlyTo(scorechangedown);//����
        }
        scoretext.text = "" + score;

    }
    private void Successe()//�ɹ����
    {
        isTrigger = false;
        successPanel.SetActive(true);
        rb.velocity = Vector2.zero;
        //scoretext.gameObject.SetActive(false);
        statistics.text = "����̶ȣ�" + ((float)score / 3).ToString("0.0") + "\n����̶ȣ�" + ((float)score / 3 - 1).ToString("0.0") + "\n����������" + ((float)score / 3 + 1).ToString("0.0");
        scoretext.text = "������" + score;
        Debug.Log("��ʱ��" + timeCount);
        DataPersistenceManager.instance.SaveGame();
        DataPersistenceManager.instance.LoadGame();
    }
    public static void FlyTo(Graphic graphic)//�������֡����Ƶģ�һ�仰��������
    {
        RectTransform rt = graphic.rectTransform;
        Color c = graphic.color;
        c.a = 0;
        graphic.color = c;
        Sequence mySequence = DOTween.Sequence();
        Tweener move1 = rt.DOMoveY(rt.position.y + 0.7f, 0.5f);
        Tweener move2 = rt.DOMoveY(rt.position.y + 1.4f, 0.5f);
        Tweener alpha1 = graphic.DOColor(new Color(c.r, c.g, c.b, 1), 0.5f);
        Tweener alpha2 = graphic.DOColor(new Color(c.r, c.g, c.b, 0), 0.5f);
        mySequence.Append(move1);
        mySequence.Join(alpha1);
        mySequence.AppendInterval(1);
        mySequence.Append(move2);
        mySequence.Join(alpha2);
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        int steel_index = PlayerPrefs.GetInt("steel_index", defaultSteelIndex);
        gameData.swords[steel_index] = true;
    }
}