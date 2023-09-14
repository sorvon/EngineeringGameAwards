using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpenManager : MonoBehaviour
{
    public GameObject[] lines;
    public GameObject[] levels;
    public GameObject nineGrid;
    public Animator clockAnimator;
    public Button nextButton;
    public float k = 0;
    public float rotateSpeed = 100;
    public float lengthSpeed = 2;
    public float tolerance = 0.01f;
    private float[] linesAngle;
    public static DoorOpenManager instance { get; private set; }

    struct Target
    {
        public float angle_1;
        public float angle_2;
        public float length;
        public Target(float a, float b, float c)
        {
            angle_1 = a;
            angle_2 = b;
            length = c;
        }
    }
    Target level_target;
    List<Target> level_targets = new List<Target>();
    Target level_1 = new Target(-44.173f, -66.26f, 10.47007f);
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        linesAngle = new float[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].transform.localRotation = Quaternion.Euler(0, 0, k);
        }
        level_targets.Add(new Target(-44.173f, -66.26f, 10.47007f));
        level_targets.Add(new Target(-20.721f, 311.651f, 10.47007f));
        level_targets.Add(new Target(-103.764f, -3.174f, 10.47007f));//-29.22 -129.76
        int level_index = Random.Range(0, levels.Length);
        level_target = level_targets[level_index];
        for (int i = 0; i < levels.Length; i++)
        {
            if (i==level_index)
            {
                levels[i].SetActive(true);
            }
            else
            {
                levels[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Horizontal"))
        {
            k -= Time.deltaTime * rotateSpeed * Input.GetAxis("Horizontal");
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].transform.localRotation = Quaternion.Euler(0, 0, k / 15 * i);
            }
        }

        if(Input.GetButton("Vertical"))
        {
            for (int i = 0; i < lines.Length; i++)
            {
                Vector3 scale = lines[i].transform.localScale;
                scale.x = Mathf.Clamp(scale.x + Time.deltaTime * Input.GetAxis("Vertical") * lengthSpeed, 5.640749f, 11);
                lines[i].transform.localScale = scale;
            }
        }
    }
    private bool nearEqualLength(float a, float b)
    {
        while (a > 360) a -= 360;
        while (b > 360) b -= 360;
        while (a < 0) a += 360;
        while (b < 0) b += 360;
        return Mathf.Abs(a - b) / b < 0.02f;
    }
    private bool nearEqual(float a, float b)
    {
        while (a > 360) a -= 360;
        while (b > 360) b -= 360;
        while (a < 0) a += 360;
        while (b < 0) b += 360;
        return Mathf.Abs(a - b) / b < tolerance;
    }
    private bool Check()
    {
        float curLength = lines[0].transform.localScale.x;
        Target target = level_target;
        if (!nearEqualLength(curLength, target.length))
        {
            print("³¤¶È²»Æ¥Åä");
            return false;
        }
        bool eq_1 = false, eq_2 = false;
        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].gameObject.activeSelf)
            {
                continue;
            }
            var angle = lines[i].transform.localRotation.eulerAngles.z;
            if (nearEqual(angle, target.angle_1))
            {
                eq_1 = true;
            }
            else if (nearEqual(angle, target.angle_2))
            {
                eq_2 = true;
            }
        }
        return eq_1 && eq_2;
    }

    public void TryOpen()
    {
        if (Check())
        {
            Animator[] animators = nineGrid.GetComponentsInChildren<Animator>(true);
            foreach (var animator in animators)
            {
                animator.enabled = true;
            }
            clockAnimator.SetBool("Start", false);
            StartCoroutine(EnableNextButton());        
        }
        else
        {

        }
    }

    public void SetClockStart(bool value)
    {
        clockAnimator.SetBool("Start", value);
    }

    IEnumerator EnableNextButton()
    {
        nextButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        nextButton.interactable = true;
    }
}
