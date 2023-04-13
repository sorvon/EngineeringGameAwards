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
    public int mine_num;

    public GameData()
    {
        skill_1 = 0;
        skill_2 = 0;
        skill_3 = 0;
        mine_num = 0;
        durationTime = 0;
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
