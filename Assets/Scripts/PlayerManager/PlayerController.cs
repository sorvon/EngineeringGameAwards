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
    private PlayerCollect playerCollect;

    private void Awake()
    {
        destroyableTiles = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        playerCollect = GetComponentInChildren<PlayerCollect>();
    }

    private void Update()
    {
        
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
        Debug.Log(collision.contacts[0].collider.name);
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
