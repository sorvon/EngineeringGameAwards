using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public int rewardvalue;
    public TextMeshProUGUI statistics;
    public TextMeshProUGUI adjusttimetext;



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
            if(PlayerPrefs.HasKey(scrollBase + i))
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
        if (adjusttime > 0) { 
        for (int i = 0; i < scrollbarList.Length; i++)
        {
            PlayerPrefs.SetFloat(scrollBase + i, scrollbarList[i].value);
        }
        adjusttime--;
        adjusttimetext.text = "ʣ�������" + adjusttime;

        rb.AddForce(new Vector2(horizontalForce, 0),ForceMode2D.Impulse);
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
        }
        if (collision.CompareTag("Punishment"))
        {
            score -= punishmentvalue;
            collision.gameObject.SetActive(false);
        }
        scoretext.text = "������" + score;
    }
    private void successes()//�ɹ����
    {
        isTrigger = false;
        successPanel.SetActive(true);
        rb.velocity = Vector2.zero;
        //scoretext.gameObject.SetActive(false);
        statistics.text = "����̶ȣ�" + (score / 3) + "\n����̶ȣ�" + (score / 3 - 1) + "\n����������" + (score / 3 + 1);
        scoretext.text = "������" + score;
    }
}
