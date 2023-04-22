using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BonusItem : MonoBehaviour
{
    public int bounsScore = 2;

    private void Start()
    {
        //transform.
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerQuench>().score += bounsScore;
            gameObject.SetActive(false);
        }
    }
}
