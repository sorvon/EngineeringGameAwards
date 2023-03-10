using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private List<GameObject> collectList;
    public List<GameObject> collectedList;
    private Rigidbody2D rb;

    private void Awake()
    {
        collectList = new List<GameObject>();
        collectedList = new List<GameObject>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (var item in collectList)
            {
                if (item.TryGetComponent<SpringJoint2D>(out var joint))
                {
                    joint.enabled = true;
                    joint.connectedBody = rb;
                    collectedList.Add(item);
                }
                if (item.TryGetComponent<LineRenderer>(out var line))
                {
                    line.enabled = true;
                    Vector3[] pos = new Vector3[2];
                    pos[0] = item.transform.position;
                    pos[1] = transform.position;
                    line.SetPositions(pos);
                }
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            foreach (var item in collectedList)
            {
                if (item.TryGetComponent<SpringJoint2D>(out var joint))
                {
                    joint.enabled = false;
                    joint.connectedBody = null;
                }
                if (item.TryGetComponent<LineRenderer>(out var line))
                {
                    line.enabled = false;
                }
            }
            collectedList.RemoveRange(0, collectedList.Count);
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            print("Fire3");
        }
        foreach (var item in collectedList)
        {
            if (item.TryGetComponent<LineRenderer>(out var line))
            {
                if (line.enabled)
                {
                    Vector3[] pos = new Vector3[2];
                    pos[0] = item.transform.position;
                    pos[1] = transform.position;
                    line.SetPositions(pos);
                }            
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collectList.Contains(collision.gameObject))
        {
            print("add:" + collision.gameObject.name);
            collectList.Add(collision.gameObject);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collectList.Contains(collision.gameObject))
        {
            print("remove:" + collision.gameObject.name);
            collectList.Remove(collision.gameObject);
        }
    }
}
