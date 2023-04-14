using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField] private GameObject successMenu;
    [SerializeField] private GameObject failMenu;
    [SerializeField] Transform[] targetList;
    void Start()
    {
        targetList = GetComponentsInChildren<Transform>();
        
    }

    public void OnFinishClicked()
    {
        var player = GameObject.FindWithTag("Player");
        var playerPos = player.transform.position;
        foreach (var target in targetList)
        {
            if (target == transform) continue;
            if((playerPos - target.position).magnitude < tolerance)
            {
                successMenu.SetActive(true);
                return;
            }
        }
        failMenu.SetActive(true);
    }
}
