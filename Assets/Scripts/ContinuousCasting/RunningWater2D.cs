using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class RunningWater2D : MonoBehaviour
{
    private new CircleCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        RunningWaterSystem2D.Add(this);
    }

    public float GetRadius()
    {
        return collider.radius;
    }

    private void OnDestroy()
    {
        RunningWaterSystem2D.Remove(this);
    }
}