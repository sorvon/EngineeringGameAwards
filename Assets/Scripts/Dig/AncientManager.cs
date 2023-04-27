using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AncientManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    List<GameObject> tiles;
    [Header("Config")]
    [SerializeField] SpriteRenderer srcSprite;
    [SerializeField] SpriteRenderer dstSprite;
    [SerializeField] GameObject ancient;

    bool isTriggered = false;  
    private void Start()
    {
        //AncientTrigger();
    }
    public void AncientTrigger()
    {
        srcSprite.DOFade(0, 1);
        dstSprite.DOFade(1, 1);
        Instantiate(ancient, transform.position, transform.rotation);
        isTriggered = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyableTile"))
        {
            tiles.Add(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isTriggered)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {
            int tileCount = tiles.Count;
            print(tiles.Count);
            foreach (var tile in tiles)
            {
                if (tile == null)
                {
                    tileCount--;
                }
            }
            if(tileCount == 0)
            {
                AncientTrigger();
            }
        }
    }
}
