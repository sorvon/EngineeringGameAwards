using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] float lengthPerParticle = 1f;
    public static LineManager instance { get; private set; }
    public float lineLength;
    LineRenderer lineRenderer;
    Vector3[] linePositions;
    private void Awake()
    {
        instance = this;
        lineLength = 0;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        linePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePositions);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterParticle"))
        {
            Destroy(collision.gameObject);
            lineLength += lengthPerParticle;
            Vector3[] curPositions = new Vector3[linePositions.Length];
            float curLength = 0;
            for (int i = 0; i < linePositions.Length - 1; i++)
            {
                var diffVec = linePositions[i + 1] - linePositions[i];
                var diffLength = diffVec.magnitude;
                curLength += diffLength;
                if (curLength > lineLength)
                {
                    lineRenderer.positionCount = i+2;
                    curPositions[i] = linePositions[i];
                    float rest = curLength - lineLength;
                    curPositions[i+1] = linePositions[i] + diffVec * (1-rest / diffLength);
                    lineRenderer.SetPositions(curPositions);
                    lineRenderer.enabled = true;
                    break;
                }
                else
                {
                    curPositions[i] = linePositions[i];
                }
            }
        }
    }
}
