using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField] private GameObject successMenu;
    [SerializeField] private GameObject failMenu;
    [SerializeField] Sprite[] normalSprites;
    [SerializeField] Sprite[] redSprites;
    [Header("Debug")]
    [SerializeField] Transform[] targetList;
    void Start()
    {
        targetList = GetComponentsInChildren<Transform>();
        
    }

    public void OnFinishClicked()
    {
        if (CheckFinish())
        {
            successMenu.SetActive(true);
        }
        else
        {
            failMenu.SetActive(true);
        }
    }

    public bool CheckFinish()
    {
        var player = GameObject.FindWithTag("Player");
        var playerPos = player.transform.position;
        int index = 0;
        bool flag = false;
        foreach (var target in targetList)
        {
            if (target == transform) continue;
            if ((playerPos - target.position).magnitude < tolerance)
            {
                flag = true;
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
}
