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

    public static PipeManager instance { get; private set; }

    float particleInterval;
    float particleTimeCount;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more then one PipeManager in the scene.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        particleInterval = 1.0f / particlePerSec;
        Time.timeScale = 0;
        //PlayerPrefs.SetInt("difficulty", 3);
    }

    private void Start()
    {
        //int difficulty = PlayerPrefs.GetInt("difficulty");
        //if (difficulty == 2)
        //{
        //    particleInterval *= 0.5f;
        //}
        //else if (difficulty == 3)
        //{
        //    particleInterval *= 0.33f;
        //}
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
