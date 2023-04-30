using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StateManager : MonoBehaviour, IDataPersistence
{
    [Header("Debug")]
    public int mineNumA;
    public int mineNumB;
    [Header(" ±º‰≈‰÷√")]
    [SerializeField] private float timeMax = 25 * 60;
    [SerializeField] private float powerMax = 5 * 60;
    [Header("UI Object")]
    [SerializeField] RectTransform timeProgress;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] RectTransform powerProgress;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI mineTextA;
    [SerializeField] TextMeshProUGUI mineTextB;
    public float timeCountdown { get; private set; }
    public float powerCountdown { get; private set; }

    void Awake()
    {
        timeCountdown = timeMax;
        powerCountdown = powerMax;
    }

    void Update()
    {
        timeCountdown -= Time.deltaTime;
        powerCountdown -= Time.deltaTime;
        timeProgress.localScale = new Vector3(timeCountdown / timeMax, 1, 1);
        timeText.text = floatToTimeString(timeCountdown);
        powerProgress.localScale = new Vector3(powerCountdown / powerMax, 1, 1);
        powerText.text = Convert.ToString(Mathf.Round(100 * powerCountdown / powerMax)) + "%";
    }

    string floatToTimeString(float value)
    {
        int minute = Mathf.FloorToInt(value / 60);
        int second = Mathf.FloorToInt(value % 60);
        return string.Format("{0:D2}:{1:D2}", minute, second);
    }

    public void PowerRecover()
    {
        powerCountdown = powerMax;
    }

    public void MineAdd(int cntA, int cntB)
    {
        mineNumA += cntA;
        mineNumB += cntB;
        mineTextA.text = Convert.ToString(mineNumA);
        mineTextB.text = Convert.ToString(mineNumB);
    }

    void IDataPersistence.LoadData(GameData gameData)
    {
        mineNumA = gameData.mineNumA;
        mineNumB = gameData.mineNumB;
    }

    void IDataPersistence.SaveData(GameData gameData)
    {
        gameData.mineNumA = mineNumA;
        gameData.mineNumB = mineNumB;
    }
}
