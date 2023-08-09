using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenManager : MonoBehaviour
{
    public GameObject[] lines;
    public float k = 0;
    public float speed = 100;
    private float[] linesAngle;
    // Start is called before the first frame update
    void Start()
    {
        linesAngle = new float[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].transform.rotation = Quaternion.Euler(0, 0, k);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Horizontal"))
        {
            k += Time.deltaTime * speed * Input.GetAxis("Horizontal");
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].transform.rotation = Quaternion.Euler(0, 0, k / 15 * i);
            }
        }

        if(Input.GetButton("Vertical"))
        {
            for (int i = 0; i < lines.Length; i++)
            {
                Vector3 scale = lines[i].transform.localScale;
                scale.x += Time.deltaTime * Input.GetAxis("Vertical");
                lines[i].transform.localScale = scale;
            }
        }
    }
}
