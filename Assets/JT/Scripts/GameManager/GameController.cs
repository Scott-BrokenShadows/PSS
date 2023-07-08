using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //SaveLoad sData;
    [ReadOnly] [SerializeField] string currentDateTime;
    [ReadOnly] [SerializeField] string currentGameplayTime;
    [ReadOnly] [SerializeField] string currency;

    void Start()
    {
        //sData = this.GetComponent<SaveLoad>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        SetRealDateTime();
        SetTotalPlayTime();
        SetCurrency();
    }

    void SetRealDateTime()
    {
        System.DateTime myTime = System.DateTime.Now;
        GameData.dateTime = myTime.ToString();
        //sData.savedData.dateTime = GameData.dateTime;
        SaveLoad.SaveSystem.savedData.dateTime = GameData.dateTime;

        currentDateTime = GameData.dateTime;
    }

    void SetTotalPlayTime()
    {
        GameData.gameplayTime = FormatTime(this.TotalTime);
        //sData.savedData.gameplayTime = GameData.gameplayTime;
        SaveLoad.SaveSystem.savedData.gameplayTime = GameData.gameplayTime;
        currentGameplayTime = GameData.gameplayTime;
    }

    #region Total Gameplay
    public float TotalTime
    {
        get
        {
            float totalTime = Time.realtimeSinceStartup;
            return totalTime;
        }
    }

    public string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    #endregion

    void SetCurrency()
    {
        currency = $"${GameData.gameCurrency.ToString()}";
    }
}
