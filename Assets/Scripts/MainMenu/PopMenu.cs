using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menu.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }
}
