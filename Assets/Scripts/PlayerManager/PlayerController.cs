using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float velocity = 10;
    public float hitInterval = 1;
    public int hitDamage = 1;
    private Rigidbody2D rb;
    private List<GameObject> destroyableTiles; 
    private float hitTimeCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        destroyableTiles = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        hitTimeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        hitTimeCount += Time.fixedDeltaTime;
        Vector2 v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = velocity * v;
        if (destroyableTiles.Count > 0 && hitTimeCount >= hitInterval && v.magnitude != 0)
        {
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
