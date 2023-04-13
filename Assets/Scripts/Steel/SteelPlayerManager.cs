using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SteelPlayerManager : MonoBehaviour
{
    [SerializeField] private StoveManager stoveManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.attachedRigidbody.tag);
        if (collision.attachedRigidbody.CompareTag("DangerEdge"))
        {
            stoveManager.OnPlayerCollision();
            transform.DOKill();
            transform.position = Vector3.zero;
        }
    }
}
