using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyableTile : MonoBehaviour
{
    public int HP = 10;
    public GameObject[] drops;
    [Header("Number of drops")]
    public int min = 1;
    public int max = 3;
    private GridLayout gridLayout;
    private Tilemap map;
    // Start is called before the first frame update
    void Start()
    {
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        map = transform.parent.GetComponentInParent<Tilemap>();
    }

    public bool SetHP(int value)
    {
        HP = value;
        if(HP <= 0)
        {
            if (drops.Length != 0)
            {
                int num = Random.Range(min, max + 1);
                for (int i = 0; i < num; i++)
                {
                    int index = Random.Range(0, drops.Length);
                    Instantiate(drops[index], transform.position, transform.rotation);
                }
            }
            
            map.SetTile(gridLayout.WorldToCell(transform.position), null);
            return true;
        }
        return false;
    }
}
