using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float velocityBase = 10;
    [SerializeField] float hitInterval = 1;
    [SerializeField] int hitDamageBase = 1;
    [SerializeField] float velocityDecreaseBase = 0.75f;
    [SerializeField] StateManager stateManager;
    [SerializeField] GameObject skillTree;
    float velocity;
    float hitDamage;
    private Rigidbody2D rb;
    private List<GameObject> destroyableTiles; 
    private PlayerCollect playerCollect;
    private bool skillTreeTrigger;

    private void Awake()
    {
        velocity = velocityBase;
        hitDamage = hitDamageBase;
        skillTreeTrigger = false;
        destroyableTiles = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        playerCollect = GetComponentInChildren<PlayerCollect>();
    }

    private void Update()
    {
        if (skillTreeTrigger)
        {
            if (Input.GetButtonDown("Fire1") && skillTree.activeSelf == false)
            {
                skillTree.SetActive(true);
            }
            else if (Input.GetButtonDown("Fire2") && skillTree.activeSelf == true)
            {
                skillTree.SetActive(false);
                
            }
        }
        if (skillTree.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        var v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = Mathf.Pow(velocityDecreaseBase, playerCollect.collectedList.Count) * velocity * v;
        if (destroyableTiles.Count > 0 && v.magnitude != 0)
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if(sprite)
            {
                sprite.color = Color.yellow;
            }
            if (destroyableTiles[^1].TryGetComponent<DestroyableTile>(out var tile))
            {
                //print(destroyableTiles.Count);
                bool isBroken = tile.SetHP(tile.HP - Time.deltaTime * hitDamage);
                if(isBroken) destroyableTiles.RemoveAt(destroyableTiles.Count - 1);
            }
            else
            {
                print("err");
            }
        }
        else
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite)
            {
                sprite.color = Color.white;
            }
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
}
