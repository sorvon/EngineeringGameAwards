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
    [SerializeField] Rigidbody2D rb;
    [SerializeField] DigDialogManager digitalDialogManager;
    // Update is called once per frame

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        digitalDialogManager = GetComponent<DigDialogManager>();
        beaconIndex = 0;
    }
    void FixedUpdate()
    {
        float axix_v = Input.GetAxis("Vertical") * moveSpeed;
        float axix_h = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector3(axix_h, axix_v, 0);
        //transform.position += new Vector3(axix_h, axix_v, 0) * Time.deltaTime * moveSpeed;
        
    }

    private void Update()
    {
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
            int less_1_cnt = 0;
            foreach (var b in beaconList)
            {
                var anim = b.GetComponent<Animator>();
                anim.Play("Begin");
                var dis = (dest.position - b.transform.position).magnitude;
                anim.SetFloat("Distance", dis);
                if (dis < 1)
                {
                    less_1_cnt++;
                }
                if(less_1_cnt >= 2)
                {
                    digitalDialogManager.ToDig();
                }
            }
        }
    }
}
