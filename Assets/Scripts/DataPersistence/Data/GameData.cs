using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public string name;
    public float durationTime;
    public string lastTime;
    public int skill_1;
    public int skill_2;
    public int skill_3;
    public int mineNumA;
    public int mineNumB;

    public int ProcessLevel = 1;
    public int ancientNum = 0;
    public int digLevel = 1;  
    
    public bool[] swords;
    public bool baseDialog = true;
    public bool skillDialog = true;
    public bool baseDigDialog = true;
    public bool steelDialog = true;
    public bool steelRollingDialog = true;
    public bool quenchDialog = true;
    public bool sudokuDialog = true;
    public bool continuousCastingDialog = true;
    public bool detectDialog = true;
    public bool[] digDialogs;
    public GameData()
    {
        skill_1 = 0;
        skill_2 = 0;
        skill_3 = 0;
        mineNumA = 0;
        mineNumB = 0;
        durationTime = 0;
        swords = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            swords[i] = false;
        }
        digDialogs = new bool[3];
        for (int i = 0; i < digDialogs.Length; i++)
        {
            digDialogs[i] = true;
        }
        lastTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    public string GetFormatDurationTime()
    {
        int hour = (int) (durationTime / 3600);
        int minute = (int) (durationTime - hour * 3600) / 60;
        int second = (int) (durationTime - hour * 3600 - minute * 60);
        return string.Format("{0}:{1:D2}:{2:D2}", hour, minute, second);
    }
}
