using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using TMPro;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Header("Config")]
    [SerializeField] float velocityBase = 10;
    [SerializeField] float hitInterval = 0.1f;
    [SerializeField] int hitDamageBase = 1;
    [SerializeField] float velocityDecreaseBase = 0.75f;
    [SerializeField] StateManager stateManager;
    [SerializeField] GameObject skillTree;
    [SerializeField] GameObject successMenu;
    [SerializeField] GameObject failMenu;
    [SerializeField] TextMeshProUGUI failText;
    [Header("音效")]
    [SerializeField] AudioClip skillOpenAudio;
    [Header("难度GameObject")]
    [SerializeField] GameObject[] tileLevels = new GameObject[3];
    [SerializeField] GameObject[] ancientLevels = new GameObject[3];
    [SerializeField] SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] SpriteRenderer baseSpriteRenderer;
    [SerializeField] SpriteRenderer gradientSpriteRenderer;
    [SerializeField] Sprite backgroundSprite;
    [SerializeField] Sprite baseSprite;
    [SerializeField] Sprite gradientSprite;
    [Header("Debug")]
    [SerializeField] float velocity;
    [SerializeField] float hitDamage;
    [SerializeField] int digLevel = 1;
    [SerializeField] int ancientNum = 0;
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
    private void Start()
    {
        if (digLevel >= 2)
        {
            backgroundSpriteRenderer.sprite = backgroundSprite;
            baseSpriteRenderer.sprite = baseSprite;
            gradientSpriteRenderer.sprite= gradientSprite;
        }
        if (digLevel > 3)
        {
            digLevel = 3;
        }
        for(int i = 0; i < tileLevels.Length; i++)
        {
            if (i+1 == digLevel)
            {
                tileLevels[i].SetActive(true);
                ancientLevels[i].SetActive(true);
            }
            else
            {
                tileLevels[i].SetActive(false);
                ancientLevels[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if ((stateManager.timeCountdown<0||stateManager.powerCountdown<0)&& !failMenu.activeSelf)
        {
            if (stateManager.powerCountdown < 0)
            {
                failText.text = "电力耗尽";
            }
            else
            {
                failText.text = "超时";
            }
            failMenu.SetActive(true);
            Time.timeScale = 0;
        }
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
        //print(v);

        for (int i = 0; i < 8; i++)
        {
            float rad = i * Mathf.Deg2Rad;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(rad*45), Mathf.Sin(rad*45)), 5f, LayerMask.GetMask("DestroyableTile"));
            if (raycastHit.collider != null)
            {
                //Debug.Log(raycastHit.collider.name);
                if (raycastHit.collider.CompareTag("DestroyableTile"))
                {
                    raycastHit.collider.GetComponent<ShadowCaster2D>().enabled = false;
                }
            }
        }


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
            if (playerCollect.HasAncient())
            {
                digLevel++;
                ancientNum++;
                DataPersistenceManager.instance.SaveGame();
                successMenu.SetActive(true);
                Time.timeScale = 0;
            }
            playerCollect.Collect(out int numA, out int numB);
            stateManager.MineAdd(numA, numB);
        }
        if (collision.CompareTag("SkillTreeTrigger"))
        {
            collision.GetComponent<SpriteRenderer>().enabled = true;
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
            collision.GetComponent<SpriteRenderer>().enabled = false;
            skillTreeTrigger = false;
        }
    }

    public void SetVelocityMultiplying(float value)
    {
        velocity = velocityBase * value;
        //print(velocity);
    }

    public void SetHitDamageMultiplying(float value)
    {
        hitDamage = hitDamageBase * value;
    }
    public void SetVelocityDecreaseBase(float value)
    {
        velocityDecreaseBase = value;
    }
    void IDataPersistence.LoadData(GameData gameData)
    {
        digLevel = gameData.digLevel;
        ancientNum = gameData.ancientNum;
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        gameData.digLevel = digLevel;
        gameData.ancientNum = ancientNum;
    }
}
