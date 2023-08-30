using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class test : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public Animator animator5;
    public GameObject panel1;
    public GameObject panel2;
    //public Image swordImage;
    public GameObject swordImage0;
    public Sprite[] swordImagesource;
    public ChangeScene changeScene;
    private bool isPLayingSuccessAnimation;
    private float successAnimationTime;
    private int animationNumber;
    // Start is called before the first frame update
    void Start()
    {
        isPLayingSuccessAnimation = false;
        swordImage0.GetComponent<Image>().sprite = swordImagesource[PlayerPrefs.GetInt("steel_index")];

    }

    // Update is called once per frame
    void Update()
    {
        if (isPLayingSuccessAnimation)
        {   successAnimationTime += Time.deltaTime;
            panel1.SetActive(true);
            if (successAnimationTime >= 1.5)
            {
                animator2.gameObject.SetActive(true);
                animator1.SetInteger("animationNumber", 2);
            }
            if (successAnimationTime >= 3)
            {
                animator3.gameObject.SetActive(true);
                animator2.SetInteger("animationNumber", 3);
            }
            if (successAnimationTime >= 4.5)
            {
                animator4.gameObject.SetActive(true);
                animator3.SetInteger("animationNumber", 4);
            }
            if (successAnimationTime >= 6)
            {
                animator5.gameObject.SetActive(true);
                animator4.SetInteger("animationNumber", 5);
                
            }
                
            if (successAnimationTime >= 7)
            {
                panel2.SetActive(true);
                swordImage0.SetActive(true);
            }
            if (successAnimationTime >= 10)
            {
                changeScene.ToScene("Base");
            }
        }
    }

    public void SuccessAnimationPlay()
    {
        isPLayingSuccessAnimation = true;
        successAnimationTime = 0;
        animationNumber = 1;
        animator1.gameObject.SetActive(true);
        PlayerPrefs.SetInt("successDialogue", PlayerPrefs.GetInt("steel_index"));
    }
    public void SceneTransferToMain()
    {
        Debug.Log("传送回基地！！");
        Debug.Log("现在刀是" + PlayerPrefs.GetInt("steel_index"));
        PlayerPrefs.SetInt("successDialogue", PlayerPrefs.GetInt("steel_index"));
        SceneManager.LoadScene("Base");

    }
}
