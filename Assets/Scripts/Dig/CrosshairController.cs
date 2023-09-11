using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float moveSpeed = 100;
    [SerializeField] GameObject beacon;
    [SerializeField] Transform dest;
    [SerializeField] GameObject successMenu;
    //[SerializeField] SpriteRenderer rulerY;
    //[SerializeField] SpriteRenderer rulerX;
    [SerializeField] Image rulerY;
    [SerializeField] Image rulerX;
    [SerializeField] float materialOffsetK = 1;
    [SerializeField] float randomRMin = 1;
    [SerializeField] float randomRMax = 3;

    [Header("Debug")]
    [SerializeField] List<GameObject> beaconList;
    [SerializeField] int beaconIndex = 0;
    Vector3 origin;
    Image crossHairImage;
    Rigidbody2D rb;
    DigDialogManager digitalDialogManager;
    // Update is called once per frame

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        crossHairImage = rb.GetComponent<Image>();
        digitalDialogManager = GetComponent<DigDialogManager>();
        beaconIndex = 0;
    }
    private void Start()
    {
        origin = transform.position;
        dest.position = dest.position + Random.Range(randomRMin, randomRMax) * (Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.right);
    }
    void FixedUpdate()
    {
        float axix_v = Input.GetAxis("Vertical") * moveSpeed;
        float axix_h = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector3(axix_h, axix_v, 0);
        var offset = transform.position - origin;

        rulerX.material.mainTextureOffset = new Vector2(offset.x * materialOffsetK, 0);
        rulerY.material.mainTextureOffset = new Vector2(0, offset.y * materialOffsetK);

        //transform.position += new Vector3(axix_h, axix_v, 0) * Time.deltaTime * moveSpeed;

    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var bPos = transform.position;
            bPos.z = 0;
            foreach (var b in beaconList)
            {
                if ((bPos - b.transform.position).magnitude < 0.5f)
                {
                    StartCoroutine(ChangeImageColor());
                    return;
                }
            }
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
                    Time.timeScale = 0;
                    successMenu.SetActive(true);
                }
            }
        }
    }

    IEnumerator ChangeImageColor()
    {
        crossHairImage.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        crossHairImage.color = Color.green;
    }
}
