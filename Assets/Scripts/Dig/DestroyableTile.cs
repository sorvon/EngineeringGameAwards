using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyableTile : MonoBehaviour
{
    public float HP = 10;
    public GameObject[] drops;
    [Header("Number of drops")]
    public int min = 1;
    public int max = 3;
   
    [Header("“Ù–ß")]
    [SerializeField] AudioClip digAudio;
    [SerializeField] AudioClip destroyAudio;

    [Header("∆∆ªµ")]
    [SerializeField] Sprite flaw_1;
    [SerializeField] Sprite flaw_2;
    [SerializeField] Sprite flaw_3;

    private GridLayout gridLayout;
    private Tilemap map;
    private AudioSource audioSource;
    private float MaxHP;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        map = transform.parent.GetComponentInParent<Tilemap>();
        MaxHP = HP;
    }

    public bool SetHP(float value)
    {
        HP = value;
        var player = GameObject.FindGameObjectWithTag("Player");
        audioSource = player.GetComponent<AudioSource>();
        if(HP <= 0)
        {
            if (drops.Length != 0)
            {
                int num = Random.Range(min, max + 1);
                for (int i = 0; i < num; i++)
                {
                    int index = Random.Range(0, drops.Length);
                    Instantiate(drops[index], 
                        transform.position + Random.Range(0, 0.1f) * Vector3.one, 
                        transform.rotation);
                }
            }
            audioSource.PlayOneShot(destroyAudio);
            //GetComponent<BoxCollider2D>().enabled = false;
            //StartCoroutine();
            map.SetTile(gridLayout.WorldToCell(transform.position), null);
            
            return true;
        }
        else
        {
            if (HP <= MaxHP / 3)
            {
                spriteRenderer.sprite = flaw_3;
            }
            else if(HP <= MaxHP * 2 / 3)
            {
                spriteRenderer.sprite = flaw_2;
            }
            else
            {
                spriteRenderer.sprite = flaw_1;
            }
            
            audioSource.PlayOneShot(digAudio);
        }
        return false;
    }
}
