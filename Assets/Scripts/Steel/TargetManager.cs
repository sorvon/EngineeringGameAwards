using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour, IDataPersistence
{
    [Header("Config")]
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField] private GameObject successMenu;
    [SerializeField] private GameObject failMenu;
    [SerializeField] Sprite[] normalSprites;
    [SerializeField] Sprite[] redSprites;
    [SerializeField] Animator barAnimator;
    [Header("Debug")]
    [SerializeField] GameObject[] targetList;
    [SerializeField] int ProcessLevel = 1;

    public void OnFinishClicked()
    {
        if (CheckFinish())
        {
            StartCoroutine(SuccessMenuEnable());
            barAnimator.SetTrigger("FinishTrigger");
        }
        //else
        //{
        //    failMenu.SetActive(true);
        //}
    }

    public bool CheckFinish()
    {
        var player = GameObject.FindWithTag("Player");
        var playerPos = player.transform.position;
        int index = 0;
        bool flag = false;
        foreach (var target in targetList)
        {
            if (index >= ProcessLevel * 2) break;
            if ((playerPos - target.transform.position).magnitude < tolerance)
            {
                flag = true;
                PlayerPrefs.SetInt("difficulty", index / 2 + 1);
                PlayerPrefs.SetInt("steel_index", index);
                target.GetComponent<SpriteRenderer>().sprite = redSprites[index];
            }
            else
            {
                target.GetComponent<SpriteRenderer>().sprite = normalSprites[index];
            }
            index++;
        }
        return flag;
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        ProcessLevel = gameData.ProcessLevel;
        for (int i = 0; i < targetList.Length; i++)
        {
            if (i < ProcessLevel * 2)
            {
                targetList[i].SetActive(true);
            }
            else
            {
                targetList[i].SetActive(false);
            }
        }
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        gameData.ProcessLevel = ProcessLevel;
    }

    IEnumerator SuccessMenuEnable()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        successMenu.SetActive(true);
    }
}
