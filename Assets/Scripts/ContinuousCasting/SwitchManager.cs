using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchManager : MonoBehaviour
{
    [SerializeField] float fallPreSec = 10;
    [SerializeField] Transform maxPos;
    [SerializeField] Transform minPos;
    [SerializeField] TextMeshProUGUI keepText;
    [SerializeField] GameObject SuccessMenu;
    public static SwitchManager instance { get; private set; } 

    Queue<CircleCollider2D> circles = new Queue<CircleCollider2D>();

    float successCount = 0;
    float fallCount = 0;
    float fallInterval;
    
    int targetSec;
    private void Awake()
    {
        fallInterval = 1 / fallPreSec;
        instance = this;
    }

    private void Start()
    {
        targetSec = 10;
        int difficulty = PlayerPrefs.GetInt("difficulty");
        if (difficulty == 2)
        {
            targetSec = 15;
        }
        else if (difficulty == 3)
        {
            targetSec = 20;
        }
    }

    private void FixedUpdate()
    {
        var water =  RunningWaterSystem2D.Get();
        float maxH = 0;
        foreach (var w in water)
        {
            if (w.GetComponent<Rigidbody2D>().velocity.y < -0.1)
            {
                continue;
            }
            maxH = Mathf.Max(maxH, w.transform.position.y);
        }
        if (maxH < maxPos.position.y && maxH > minPos.position.y)
        {
            successCount+=Time.deltaTime;
        }
        else
        {
            if (targetSec == 15)
            {
                successCount -= Time.deltaTime * 2;
            }
            else if(targetSec == 20)
            {
                successCount -= Time.deltaTime * 4;
            }
            
        }
        successCount = Mathf.Max(successCount, 0);
        keepText.text = string.Format("±£³Ö: {0:0.0} / {1}", successCount, targetSec);
        if (successCount > 0)
        {
            keepText.enabled = true;
        }
        else
        {
            keepText.enabled = false;
        }
        if (successCount>=targetSec)
        {
            Time.timeScale = 0;
            SuccessMenu.SetActive(true);
        }
        //print(successCount);
    }

    // Update is called once per frame
    void Update()
    {
        fallCount+=Time.deltaTime;
        if (fallCount > fallInterval && circles.Count > 0)
        {
            circles.Peek().gameObject.layer = 11;
            circles.Dequeue();
            fallCount = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<CircleCollider2D>(out var circle))
        {
            circles.Enqueue(circle);
        };
    }

    //private IEnumerator CircleEnable(CircleCollider2D circle)
    //{
    //    yield return new WaitForSeconds(1);
    //    circle.enabled = true;
    //}
}
