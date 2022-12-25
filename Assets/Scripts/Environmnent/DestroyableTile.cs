using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyableTile : MonoBehaviour
{
    public int HP = 10;
    
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
            map.SetTile(gridLayout.WorldToCell(transform.position), null);
            return true;
        }
        return false;
    }
}
