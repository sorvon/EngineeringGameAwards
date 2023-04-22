using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class PlayerQuench : MonoBehaviour
{
    public float horizontalForce = 5;
    public float verticalForce = 1;
    public float verticalForceInterval = 1;
    public float destTolerance = 1;
    public Scrollbar[] scrollbarList;
    static double[] scrollbarValues;
    public Transform destination;
    public GameObject successPanel;
    public GameObject failurePanel;
    public Text scoretext;
    public int punishmentvalue;
    public int rewardvalue;                   //增益加分分值
    public TextMeshProUGUI statistics;        //表面质量等包装数据
    public TextMeshProUGUI adjusttimetext;    //剩余调整次数
    //分数跳字组件
    public TextMeshProUGUI scorechangeup;       
    public TextMeshProUGUI scorechangedown;
    public TextMeshProUGUI plus3;


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
        score = 5;
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
        adjusttimetext.text = "剩余次数：" + adjusttime;

    }

    void Update()
    {
        if (isTrigger)
        {
            timeCount += Time.deltaTime;
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
        if (adjusttime > 0)
        {
            for (int i = 0; i < scrollbarList.Length; i++)
            {
                PlayerPrefs.SetFloat(scrollBase + i, scrollbarList[i].value);
            }
            adjusttime--;
            adjusttimetext.text = "剩余次数：" + adjusttime;

            rb.AddForce(new Vector2(horizontalForce, 0), ForceMode2D.Impulse);
            timeCount = 0;
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
        }
        if (collision.collider.CompareTag("Reward2"))
        {
            score += 2;
            Successe();
        }
        if (collision.collider.CompareTag("Reward3"))
        {
            score += 3;
            Successe();
            //跳字+3
            plus3.rectTransform.position = transform.position;
            FlyTo(plus3);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//增益与障碍触碰
    {
        if (collision.CompareTag("Reward"))
        {
            score += rewardvalue;
            collision.gameObject.SetActive(false);
            scorechangeup.gameObject.SetActive(true);
            scorechangeup.rectTransform.position = transform.position;
            FlyTo(scorechangeup);//跳字
        }
        if (collision.CompareTag("Punishment"))
        {
            score -= punishmentvalue;
            collision.gameObject.SetActive(false);
            scorechangedown.gameObject.SetActive(true);
            scorechangedown.rectTransform.position = transform.position;
            FlyTo(scorechangedown);//跳字
        }
        scoretext.text = "分数：" + score;

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
    /*public void PositionConvert()
    {
        // 将obj1的世界坐标转换为屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(obj1.transform.position);

        // 将屏幕坐标转换为ui坐标
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(obj2, screenPos, null, out localPos);

        // 将ui坐标赋值给obj2的anchoredPosition3D属性
        scorechangedown.transform.position = localPos;
    }*/

}