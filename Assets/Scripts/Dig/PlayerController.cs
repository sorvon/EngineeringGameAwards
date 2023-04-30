using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float velocityBase = 10;
    [SerializeField] float hitInterval = 0.1f;
    [SerializeField] int hitDamageBase = 1;
    [SerializeField] float velocityDecreaseBase = 0.75f;
    [SerializeField] StateManager stateManager;
    [SerializeField] GameObject skillTree;
    [Header("“Ù–ß")]
    [SerializeField] AudioClip skillOpenAudio;
    float velocity;
    float hitDamage;
    private Rigidbody2D rb;
    private List<GameObject> destroyableTiles; 
    private PlayerCollect playerCollect;
    private Animator playerAnimator;
    private bool skillTreeTrigger;
    private float hitIntervalTimeCount;
    private bool moveLock;
    private AudioSource audioSource;

    private void Awake()
    {
        velocity = velocityBase;
        hitDamage = hitDamageBase;
        moveLock = false;
        hitIntervalTimeCount = 0;
        skillTreeTrigger = false;
        destroyableTiles = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        playerCollect = GetComponentInChildren<PlayerCollect>();
        playerAnimator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (skillTreeTrigger)
        {
            if (Input.GetButtonDown("Fire1") && skillTree.activeSelf == false)
            {
                audioSource.PlayOneShot(skillOpenAudio);
                skillTree.SetActive(true);
                //Time.timeScale = 0;
            }
            else if (Input.GetButtonDown("Fire2") && skillTree.activeSelf == true)
            {
                skillTree.SetActive(false);
                //Time.timeScale = 1;
            }
        }
        
    }
    private void FixedUpdate()
    {
        hitIntervalTimeCount += Time.fixedDeltaTime;
        var v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        print(v);
        if (! moveLock)
        {
            rb.velocity = Mathf.Pow(velocityDecreaseBase, playerCollect.collectedList.Count) * velocity * v;
        }
        
        if (destroyableTiles.Count > 0 && v.magnitude != 0 && hitIntervalTimeCount > hitInterval)
        {
            hitIntervalTimeCount = 0;
            //var sprite = GetComponentInChildren<SpriteRenderer>();
            //if(sprite)
            //{
            //    sprite.color = Color.yellow;
            //}
            if (destroyableTiles[^1].TryGetComponent<DestroyableTile>(out var tile))
            {
                //print(destroyableTiles.Count);
                var vecDir = (tile.transform.position - transform.position).normalized;
                float angleCos = Vector3.Dot(vecDir, Vector3.right);
                if (angleCos >= 1 / Mathf.Sqrt(2))
                {
                    playerAnimator.SetTrigger("HitRight");
                }
                else if (angleCos <= -1 / Mathf.Sqrt(2))
                {
                    playerAnimator.SetTrigger("HitLeft");
                }
                else if (vecDir.y > 0)
                {
                    playerAnimator.SetTrigger("HitUp");
                }
                else
                {
                    playerAnimator.SetTrigger("HitDown");
                }
                //rb.velocity = Vector2.zero;
                //rb.AddForce(-vecDir * 10, ForceMode2D.Impulse);
                moveLock = true;
                rb.velocity = Vector2.zero;
                rb.DOMove(new Vector2(-vecDir.x, -vecDir.y)*0.1f, 0.5f).SetRelative(true).OnComplete(() =>
                {
                    //Input.ResetInputAxes();
                    
                    moveLock = false;
                });
                bool isBroken = tile.SetHP(tile.HP - hitDamage);
                if (isBroken) destroyableTiles.RemoveAt(destroyableTiles.Count - 1);
            }
            else
            {
                print("err");
            }
        }
        else
        {
            //var sprite = GetComponentInChildren<SpriteRenderer>();
            //if (sprite)
            //{
            //    sprite.color = Color.white;
            //}
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.contacts[0].collider.name);
        if (collision.gameObject.CompareTag("DestroyableTile"))
        {
            if (!destroyableTiles.Contains(collision.gameObject))
            {
                destroyableTiles.Add(collision.gameObject);
            }
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DestroyableTile"))
        {
            if (destroyableTiles.Contains(collision.gameObject))
            {
                destroyableTiles.Remove(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MineCollector"))
        {
            playerCollect.Collect(out int numA, out int numB);
            stateManager.MineAdd(numA, numB);
        }
        if (collision.CompareTag("SkillTreeTrigger"))
        {
            skillTreeTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Base"))
        {
            stateManager.PowerRecover();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SkillTreeTrigger"))
        {
            skillTreeTrigger = false;
        }
    }

    public void SetVelocityMultiplying(float value)
    {
        velocity = velocityBase * value;
        print(velocity);
    }
}
