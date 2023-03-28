using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int score;
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

        for (int i = 0; i < scrollbarList.Length; i++)
        {
            if(PlayerPrefs.HasKey(scrollBase + i))
            {
                scrollbarList[i].value = PlayerPrefs.GetFloat(scrollBase + i);
            }
        }
    }

    void Update()
    {
        if (isTrigger)
        {
            timeCount += Time.deltaTime;
        }
        if (Vector3.Distance(transform.position, destination.position) < destTolerance)
        {
            isTrigger = false;
            successPanel.SetActive(true);
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isTrigger)
        {
            int index = (int)(timeCount / verticalForceInterval);
            if (index < scrollbarList.Length)
            {
                float froce = (scrollbarList[index].value - 0.5f) * 2 * verticalForce;
                rb.AddForce(new Vector2(0, froce));
            }
        }
    }


    public void Trigger()
    {
        //rb.velocity = new Vector2(5, 0);
        for (int i = 0; i < scrollbarList.Length; i++)
        {
            PlayerPrefs.SetFloat(scrollBase + i, scrollbarList[i].value);
        }
        
        rb.AddForce(new Vector2(horizontalForce, 0),ForceMode2D.Impulse);
        timeCount = 0;
        isTrigger = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.CompareTag("DangerEdge"))
        {
            print("ÓÃÊ±£º" + timeCount + " Ãë");
            isTrigger = false;
            transform.position = initPos;
            rb.velocity = Vector2.zero;
            failurePanel.SetActive(true);
        }
    }

}
