using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DigSkillTreeManager : MonoBehaviour
{
    [Header("Skill Level")]
    public int skill_1 = 0;
    [SerializeField] GameObject skill_ui_1;
    public int skill_2 = 0;
    [SerializeField] GameObject skill_ui_2;
    public int skill_3 = 0;
    [SerializeField] GameObject skill_ui_3;
    [SerializeField] StateManager stateManager;
    [SerializeField] PlayerController player;

    private void Start()
    {
        var buttons_1 = skill_ui_1.GetComponentsInChildren<Button>();
        var buttons_2 = skill_ui_2.GetComponentsInChildren<Button>();
        var buttons_3 = skill_ui_3.GetComponentsInChildren<Button>();
        foreach (var button in buttons_1)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                var requireNum = 2 + 2 * skill_1;
                if (stateManager.mineNumA >= requireNum && stateManager.mineNumB >= requireNum)
                {
                    stateManager.MineAdd(-requireNum, -requireNum);
                    button.interactable = false;
                    skill_1++;
                    SkillUpdata();
                }
            });
        }
        foreach (var button in buttons_2)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                var requireNum = 2 + 2 * skill_2;
                if (stateManager.mineNumA >= requireNum && stateManager.mineNumB >= requireNum)
                {
                    stateManager.MineAdd(-requireNum, -requireNum);
                    button.interactable = false;
                    skill_2++;
                    SkillUpdata();
                }
            });
        }
        foreach (var button in buttons_3)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                var requireNum = 2 + 2 * skill_3;
                if (stateManager.mineNumA >= requireNum && stateManager.mineNumB >= requireNum)
                {
                    stateManager.MineAdd(-requireNum, -requireNum);
                    button.interactable = false;
                    skill_3++;
                    SkillUpdata();
                }
            });
        }
        SkillUpdata();
    }
    void SkillUpdata()
    {
        var buttons_1 = skill_ui_1.GetComponentsInChildren<Button>();
        var buttons_2 = skill_ui_2.GetComponentsInChildren<Button>();
        var buttons_3 = skill_ui_3.GetComponentsInChildren<Button>();
        if (skill_1 < buttons_1.Length)
        {
            buttons_1[skill_1].interactable = true;
        }
        if (skill_2 < buttons_2.Length)
        {
            buttons_2[skill_2].interactable = true;
        }
        if (skill_3 < buttons_3.Length)
        {
            buttons_3[skill_3].interactable = true;
        }
        if (skill_2 == 1)
        {
            player.SetVelocityMultiplying(1.25f);
        }
        else if (skill_2 == 2)
        {
            player.SetVelocityMultiplying(1.5375f);
        }
        else if (skill_2 == 3)
        {
            player.SetVelocityMultiplying(1.891125f);
        }

        if (skill_1 == 1)
        {
            player.SetHitDamageMultiplying(4f);
        }
        else if (skill_1 == 2)
        {
            player.SetHitDamageMultiplying(9f);
        }
        else if (skill_1 == 3)
        {
            player.SetHitDamageMultiplying(14f);
        }

        if (skill_3 == 1)
        {
            player.SetVelocityDecreaseBase(0.8f);
        }
        else if (skill_3 == 2)
        {
            player.SetVelocityDecreaseBase(0.85f);
        }
        else if (skill_3 == 3)
        {
            player.SetVelocityDecreaseBase(0.9f);
        }
    }
    public void OnBackClicked()
    {
        gameObject.SetActive(false);
    }
}
