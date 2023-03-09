using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingQTE : MonoBehaviour
{
    [SerializeField]
    float interval = 1;
    [SerializeField]
    float r_min = 50;
    [SerializeField]
    float r_max = 100;
    [SerializeField]
    float r_light = 80;
    [SerializeField]
    GameObject bars;
    [SerializeField]
    GameObject lights;
    
    bool[] state;
    float timeCount;
    int currentIndex;
    bool direction;

    void Awake()
    {
        timeCount = 0;
        state = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            state[i] = false;
            GameObject bar = bars.transform.GetChild(i).gameObject;
            GameObject light = lights.transform.GetChild(i).gameObject;
            bar.transform.localPosition = new Vector3(-r_max * Mathf.Sin(i * Mathf.PI / 4), r_max * Mathf.Cos(i * Mathf.PI / 4), 0);
            light.transform.localPosition = new Vector3(-r_light * Mathf.Sin(i * Mathf.PI / 4), r_light * Mathf.Cos(i * Mathf.PI / 4), 0);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= interval)
        {
            if (direction)
            {
                currentIndex++;
            }
            else
            {
                currentIndex--;
            }
            if (currentIndex >= 8)
            {
                currentIndex = 0;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 7;
            }
            timeCount = 0;
        } 
        
        for (int i = 0; i < 8; i++)
        {
            GameObject bar = bars.transform.GetChild(i).gameObject;
            GameObject light = lights.transform.GetChild(i).gameObject;
            Image img = light.GetComponent<Image>();
            if (currentIndex == i)
            {
                img.color = Color.red;
            }
            else
            {
                img.color = state[i] ? Color.green : Color.black;
            }
            float r_aim;
            if (state[i])
            {
                r_aim = r_min;
            }
            else 
            {
                r_aim = r_max;
            }
            bar.transform.localPosition = new Vector3(-r_aim * Mathf.Sin(i * Mathf.PI / 4), r_aim * Mathf.Cos(i * Mathf.PI / 4), 0);
        }
    }

    public void RollingTrigger()
    {
        state[currentIndex] = !state[currentIndex];
        direction = !direction;
    }
}
