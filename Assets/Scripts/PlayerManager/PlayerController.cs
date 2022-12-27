using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float velocity = 10;
    public float hitInterval = 1;
    public int hitDamage = 1;
    public float velocityDecreaseBase = 0.75f;
    private Rigidbody2D rb;
    private List<GameObject> destroyableTiles; 
    private float hitTimeCount = 0;
    private PlayerCollect playerCollect;
    // Start is called before the first frame update
    void Start()
    {
        destroyableTiles = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        hitTimeCount = 0;
        playerCollect = GetComponentInChildren<PlayerCollect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        hitTimeCount += Time.fixedDeltaTime;
        var v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = Mathf.Pow(velocityDecreaseBase, playerCollect.collectedList.Count) * velocity * v;
        if (destroyableTiles.Count > 0 && v.magnitude != 0)
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if(sprite)
            {
                sprite.color = Color.yellow;
            }
            if (hitTimeCount < hitInterval) return;
            if (destroyableTiles[^1].TryGetComponent<DestroyableTile>(out var tile))
            {
                hitTimeCount = 0;
                print(destroyableTiles.Count);
                bool isBroken = tile.SetHP(tile.HP - hitDamage);
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
}
