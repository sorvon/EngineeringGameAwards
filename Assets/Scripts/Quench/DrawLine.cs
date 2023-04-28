using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject player;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    private LineRenderer lineRenderer;
    private float counter;
    private float dist;
    PlayerQuench playerQuench;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = 0;
        counter = 0;
        dist = 0;
        playerQuench = GetComponent<PlayerQuench>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerQuench.isTrigger)
        {
            lineRenderer.positionCount = (int)counter + 1;
            lineRenderer.SetPosition((int)counter, player.transform.position);
            counter += 0.1f;
            dist += 0.1f;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            lineRenderer.colorGradient = gradient;
        }
    }
}
