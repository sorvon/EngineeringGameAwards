using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class ChangeScene : MonoBehaviour, IDataPersistence
{
    [SerializeField] Animator animator;
    private Image[] swordImages;
    private void Awake()
    {
        swordImages = animator.gameObject.GetComponentsInChildren<Image>(true);
    }
    public void ToScene(string name)
    {
        PlayerQuench.adjusttime = 5;
        animator.SetTrigger("Start");
        StartCoroutine(LoadScene(name));
    }
    public void ToSteelRollingOrContinuousCasting()
    {
        PlayerQuench.adjusttime = 5;
        animator.SetTrigger("Start");
        var index = PlayerPrefs.GetInt("steel_index");
        if (index % 2 == 0)
        {
            StartCoroutine(LoadScene("SteelRolling"));
        }
        else
        {
            StartCoroutine(LoadScene("ContinuousCasting"));
        }
        
    }
    public void ReloadScene()
    {
        animator.SetTrigger("Start");
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }

    IEnumerator LoadScene(string name)
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(name);
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        bool[] swords = gameData.swords;
        for (int i = 0; i < swords.Length; i++)
        {
            if (swords[i])
            {
                swordImages[i+2].enabled = false;
                swordImages[i+10].enabled = true;
            }
            else
            {
                swordImages[i + 2].enabled = true;
                swordImages[i + 10].enabled = false;
            }
        }
    }

    void IDataPersistence.SaveData(GameData gameData)
    {

    }
}
