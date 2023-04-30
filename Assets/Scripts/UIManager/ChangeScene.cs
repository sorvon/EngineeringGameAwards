using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class ChangeScene : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void ToScene(string name)
    {
        PlayerQuench.adjusttime = 5;
        animator.SetTrigger("Start");
        StartCoroutine(LoadScene(name));
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
}
