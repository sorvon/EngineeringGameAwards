using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] Vector2 direction;
    [SerializeField] float angle;
    [SerializeField] float particlePerSec;
    [SerializeField] GameObject particlePrefab;

    float particleInterval;
    float particleTimeCount;

    // Start is called before the first frame update
    void Start()
    {
        particleInterval = 1.0f / particlePerSec;
    }

    // Update is called once per frame
    void Update()
    {
        particleTimeCount += Time.deltaTime;
        if(Input.GetButton("Fire1") && particleTimeCount >= particleInterval)
        {
            particleTimeCount = 0;
            particleInterval = 1.0f / particlePerSec;
            var particle = GameObject.Instantiate(particlePrefab, transform.position, transform.rotation);
            if (particle.TryGetComponent<Rigidbody2D>(out var rb))
            {
                var ang = Random.Range(-angle, angle);
                var dir = Quaternion.AngleAxis(ang, new Vector3(0, 0, 1)) * direction;
                rb.velocity = dir;
            }
        }
    }
}
