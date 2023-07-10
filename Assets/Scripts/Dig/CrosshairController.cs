using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float moveSpeed = 100;
    [SerializeField] GameObject beacon;
    [SerializeField] Transform dest;

    [Header("Debug")]
    [SerializeField] List<GameObject> beaconList;
    [SerializeField] int beaconIndex = 0;
    // Update is called once per frame

    private void Awake()
    {
        beaconIndex = 0;
    }
    void Update()
    {
        float axix_v = Input.GetAxis("Vertical");
        float axix_h = Input.GetAxis("Horizontal");
        transform.position += new Vector3(axix_h, axix_v, 0) * Time.deltaTime * moveSpeed;
        if (Input.GetButtonDown("Fire1"))
        {
            var bPos = transform.position;
            bPos.z = 0;
            if (beaconList.Count < 3)
            {
                beaconList.Add(GameObject.Instantiate(beacon, bPos, transform.rotation));
                beaconIndex += 1;
                beaconIndex %= 3;
            }
            else
            {
                beaconList[beaconIndex].transform.position = bPos;
                beaconIndex += 1;
                beaconIndex %= 3;
            }
            foreach (var b in beaconList)
            {
                var anim = b.GetComponent<Animator>();
                anim.Play("Begin");
                
                anim.SetFloat("Distance", (dest.position - b.transform.position).magnitude);
            }
        }
    }
}