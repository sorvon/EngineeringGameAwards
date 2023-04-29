using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;
public class RollingQTE : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] bool overrideDifficulty;
    [SerializeField] int defaultDifficulty = 1;
    [Header("Easy")]
    [SerializeField] float interval_1 = 1;
    [SerializeField] int barNum_1 = 5;
    [Header("Normal")]
    [SerializeField] float interval_2 = 0.7f;
    [SerializeField] int barNum_2 = 6;
    [Header("Hard")]
    [SerializeField] float interval_3 = 0.4f;
    [SerializeField] int barNum_3 = 7;
    [Header("Config")]
    [SerializeField] float moveDistance = -150;
    [SerializeField] GameObject bars;
    [SerializeField] GameObject lights;
    [SerializeField] GameObject bases;
    [SerializeField] float timeCountdown = 30f;
    [SerializeField] GameObject steelBar;
    [SerializeField] GameObject successMenu;
    [SerializeField] GameObject failMenu;
    List<GameObject> barList = new();
    List<GameObject> lightList = new();
    List<GameObject> baseList = new();
    float interval;
    int barNum;

    bool[] state;
    float timeCount;
    int currentIndex;
    bool direction;

    void Awake()
    {
        Time.timeScale = 1;
        timeCount = 0;
        direction = true;
        if (overrideDifficulty)
        {
            PlayerPrefs.SetInt("difficulty", defaultDifficulty);
        }
        int difficulty = PlayerPrefs.GetInt("difficulty", defaultDifficulty);
        switch (difficulty)
        {
            case 1:
                interval = interval_1;
                barNum = barNum_1;
                break;
            case 2:
                interval = interval_2;
                barNum = barNum_2;
                break;
            case 3:
                interval = interval_3;
                barNum = barNum_3;
                break;
            default:
                Debug.LogError("Unknown difficulty");
                break;
        }
        state = new bool[barNum];
        int[] shuffleArr = { 0, 1, 2, 3, 4, 5, 6, 7 };
        for (int i = shuffleArr.Length - 1; i > 0; i--)
        {
            int index = Random.Range(0, i + 1);
            (shuffleArr[i], shuffleArr[index]) = (shuffleArr[index], shuffleArr[i]);
        }
        Array.Sort(shuffleArr, 0, barNum);
        for (int i = 0; i < 8; i++)
        {
            if (i < barNum)
            {
                state[i] = false;
                barList.Add(bars.transform.GetChild(shuffleArr[i]).gameObject);
                lightList.Add(lights.transform.GetChild(shuffleArr[i]).gameObject);
                baseList.Add(bases.transform.GetChild(shuffleArr[i]).gameObject);
            }
            else
            {
                bars.transform.GetChild(shuffleArr[i]).gameObject.transform.DOLocalMoveY(moveDistance, 0.0f);
                lights.transform.GetChild(shuffleArr[i]).gameObject.GetComponent<Image>().color = Color.blue ;
                //bases.transform.GetChild(shuffleArr[i]).gameObject.SetActive(false);
            }
            //GameObject bar = bars.transform.GetChild(i).gameObject;
            //GameObject light = lights.transform.GetChild(i).gameObject;
            //bar.transform.localPosition = new Vector3(-r_max * Mathf.Sin(i * Mathf.PI / 4), r_max * Mathf.Cos(i * Mathf.PI / 4), 0);
            //light.transform.localPosition = new Vector3(-r_light * Mathf.Sin(i * Mathf.PI / 4), r_light * Mathf.Cos(i * Mathf.PI / 4), 0);
        }
        
    }
    private void Start()
    {
        steelBar.GetComponent<RectTransform>().DOAnchorPosX(0, timeCountdown);
    }

    // Update is called once per frame
    void Update()
    {
        if (successMenu.activeSelf == true || failMenu.activeSelf == true)
        {
            return;
        }
        timeCountdown -= Time.deltaTime;
        if (timeCountdown < 0)
        {
            failMenu.SetActive(true);
            steelBar.transform.DOKill();
        }
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
            if (currentIndex >= barNum)
            {
                currentIndex = 0;
            }
            else if (currentIndex < 0)
            {
                currentIndex = barNum-1;
            }
            timeCount = 0;
        } 
        
        for (int i = 0; i < lightList.Count; i++)
        {
            Image img = lightList[i].GetComponent<Image>();
            if (currentIndex == i)
            {
                img.color = Color.white;
            }
            else
            {
                img.color = state[i] ? Color.green : Color.black;
            }
        }
    }

    public void RollingTrigger()
    {
        if (successMenu.activeSelf == true || failMenu.activeSelf == true)
        {
            return;
        }
        state[currentIndex] = !state[currentIndex];
        direction = !direction;
        int successCount = 0;
        for (int i = 0; i < barList.Count; i++)
        {
            if (state[i])
            {
                barList[i].transform.DOLocalMoveY(moveDistance, 0.5f);
                successCount++;
            }
            else
            {
                barList[i].transform.DOLocalMoveY(0, 0.5f);
            }
        }
        if (successCount == barList.Count)
        {
            successMenu.SetActive(true);
            steelBar.transform.DOKill();
        }
    }
}
