using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MaxSwitch : MonoBehaviour
{
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("WaterParticle"))
    //    {
    //        if (collision.GetComponent<Rigidbody2D>().velocity.y < -0.01)
    //        {
    //            return;
    //        }
    //        Interlocked.Increment(ref SwitchManager.instance.maxCount);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("WaterParticle"))
    //    {
    //        Interlocked.Decrement(ref SwitchManager.instance.maxCount);
    //        if (SwitchManager.instance.maxCount < 0)
    //        {
    //            Interlocked.Exchange(ref SwitchManager.instance.maxCount, 0);
    //        }
    //    }
    //}
}
