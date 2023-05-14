using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class PlayerQuench : MonoBehaviour, IDataPersistence
{
    [Header("音频")]
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
    public int rewardvalue;                   //增益加分分值
    public TextMeshProUGUI statistics;        //表面质量等包装数据
    public TextMeshProUGUI adjusttimetext;    //剩余调整次数
    [Header("分数跳字组件")]
    //分数跳字组件
    public TextMeshProUGUI scorechangeup;       
    public TextMeshProUGUI scorechangedown;
    public TextMeshProUGUI plus3;
    public TextMeshProUGUI timetext;

    [Header("难度GameObject")]
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
        //继承前一次设定的滚条数据
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
        //成功淬火
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
        if (isTrigger)//根据滚条位置施加相应大小的力
        {
            int index = (int)(timeCount / verticalForceInterval);
            if (index < scrollbarList.Length)
            {
                float froce = (scrollbarList[index].value - 0.5f) * 2 * verticalForce;
                rb.AddForce(new Vector2(0, froce));
            }
        }
    }


    public void Trigger()//开始淬火
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

    private void OnCollisionEnter2D(Collision2D collision)//到达终点
    {

        if (collision.collider.CompareTag("DangerEdge"))
        {
            print("用时：" + timeCount + " 秒");
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
            //跳字+3
            plus3.rectTransform.position = transform.position;
            FlyTo(plus3);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)//增益与障碍触碰
    {

        if (collision.CompareTag("Reward"))
        {
            scoreUpSound.Play();
            score += rewardvalue;
            collision.gameObject.SetActive(false);
            scorechangeup.gameObject.SetActive(true);
            scorechangeup.rectTransform.position = transform.position;
            FlyTo(scorechangeup);//跳字
            
        }
        if (collision.CompareTag("Punishment"))
        {
            scoreDownSound.Play();
            score -= punishmentvalue;
            collision.gameObject.SetActive(false);
            scorechangedown.gameObject.SetActive(true);
            scorechangedown.rectTransform.position = transform.position;
            FlyTo(scorechangedown);//跳字
        }
        scoretext.text = "" + score;

    }
    private void Successe()//成功淬火
    {
        isTrigger = false;
        successPanel.SetActive(true);
        rb.velocity = Vector2.zero;
        //scoretext.gameObject.SetActive(false);
        statistics.text = "烧损程度：" + ((float)score / 3).ToString("0.0") + "\n切损程度：" + ((float)score / 3 - 1).ToString("0.0") + "\n表面质量：" + ((float)score / 3 + 1).ToString("0.0");
        scoretext.text = "分数：" + score;
        Debug.Log("用时：" + timeCount);
        DataPersistenceManager.instance.SaveGame();
        DataPersistenceManager.instance.LoadGame();
    }
    public static void FlyTo(Graphic graphic)//用于跳字。复制的，一句话都看不懂
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