using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]float fallPreSec = 10;

    Queue<CircleCollider2D> circles = new Queue<CircleCollider2D>();
    
    float fallCount = 0;
    float fallInterval;
    // Start is called before the first frame update
    void Start()
    {
        fallInterval = 1 / fallPreSec;
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
