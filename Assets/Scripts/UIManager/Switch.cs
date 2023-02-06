using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour 
{
 
    public void tick(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
