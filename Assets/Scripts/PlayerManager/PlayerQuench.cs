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
    public int rewardvalue;                   //����ӷַ�ֵ
    public TextMeshProUGUI statistics;        //���������Ȱ�װ����
    public TextMeshProUGUI adjusttimetext;    //ʣ���������
    public TextMeshProUGUI scorechangeup;       //��������
    public TextMeshProUGUI scorechangedown;
    //public GameObject target;
    public GameObject obj1;
    public RectTransform obj2;


    public int score;
    public static int adjusttime = 5;
    string scrollBase;
    bool isTrigger;
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
        //�̳�ǰһ���趨�Ĺ�������
        for (int i = 0; i < scrollbarList.Length; i++)
        {
            if (PlayerPrefs.HasKey(scrollBase + i))
            {
                scrollbarList[i].value = PlayerPrefs.GetFloat(scrollBase + i);
            }
        }
        adjusttimetext.text = "ʣ�������" + adjusttime;

    }

    void Update()
    {
        if (isTrigger)
        {
            timeCount += Time.deltaTime;
        }
        /*if (Vector3.Distance(transform.position, destination.position) < destTolerance)//�ɹ����
        {
            *//*isTrigger = false;
            successPanel.SetActive(true);
            rb.velocity = Vector2.zero;*//*
            successes();
        }*/
        // ��obj1����������ת��Ϊ��Ļ����
        //positionconvert();
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
        if (adjusttime > 0)
        {
            for (int i = 0; i < scrollbarList.Length; i++)
            {
                PlayerPrefs.SetFloat(scrollBase + i, scrollbarList[i].value);
            }
            adjusttime--;
            adjusttimetext.text = "ʣ�������" + adjusttime;

            rb.AddForce(new Vector2(horizontalForce, 0), ForceMode2D.Impulse);
            timeCount = 0;
            isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
            successes();
        }
        if (collision.collider.CompareTag("Reward2"))
        {
            score += 2;
            successes();
        }
        if (collision.collider.CompareTag("Reward3"))
        {
            score += 3;
            successes();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//�������ϰ�����
    {
        if (collision.CompareTag("Reward"))
        {
            score += rewardvalue;
            collision.gameObject.SetActive(false);
            scorechangeup.gameObject.SetActive(true);
            FlyTo(scorechangeup);//����
        }
        if (collision.CompareTag("Punishment"))
        {
            score -= punishmentvalue;
            collision.gameObject.SetActive(false);
            scorechangedown.gameObject.SetActive(true);
            FlyTo(scorechangedown);//����
        }
        scoretext.text = "������" + score;
        //positionconvert();

    }
    private void successes()//�ɹ����
    {
        isTrigger = false;
        successPanel.SetActive(true);
        rb.velocity = Vector2.zero;
        //scoretext.gameObject.SetActive(false);
        statistics.text = "����̶ȣ�" + ((float)score / 3).ToString("0.0") + "\n����̶ȣ�" + ((float)score / 3 - 1).ToString("0.0") + "\n����������" + ((float)score / 3 + 1).ToString("0.0");
        scoretext.text = "������" + score;
    }
    public static void FlyTo(Graphic graphic)//�������֡����Ƶģ�һ�仰��������
    {
        RectTransform rt = graphic.rectTransform;
        Color c = graphic.color;
        c.a = 0;
        graphic.color = c;
        Sequence mySequence = DOTween.Sequence();
        Tweener move1 = rt.DOMoveY(rt.position.y + 50, 0.5f);
        Tweener move2 = rt.DOMoveY(rt.position.y + 100, 0.5f);
        Tweener alpha1 = graphic.DOColor(new Color(c.r, c.g, c.b, 1), 0.5f);
        Tweener alpha2 = graphic.DOColor(new Color(c.r, c.g, c.b, 0), 0.5f);
        mySequence.Append(move1);
        mySequence.Join(alpha1);
        mySequence.AppendInterval(1);
        mySequence.Append(move2);
        mySequence.Join(alpha2);
    }
    public void positionconvert()
    {
        // ��obj1����������ת��Ϊ��Ļ����
        Vector3 screenPos = Camera.main.WorldToScreenPoint(obj1.transform.position);

        // ����Ļ����ת��Ϊui����
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(obj2, screenPos, null, out localPos);

        // ��ui���긳ֵ��obj2��anchoredPosition3D����
        //obj2.transform.position = localPos;
    }

}