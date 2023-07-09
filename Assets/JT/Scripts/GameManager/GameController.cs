using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public HBCharacterBase _cUnit;
    [Separator]
    [ReadOnly] [SerializeField] string currentDateTime;
    [ReadOnly] [SerializeField] string currentGameplayTime;
    [ReadOnly] [SerializeField] string currency;
    [Separator]
    [ReadOnly] [SerializeField] List<UserCharactersSlot> listCharacters;
    [ReadOnly] [SerializeField] List<UserCharactersSlot> listPartyBattleUnits;
    [ReadOnly] [SerializeField] List<ItemSlot> listItems;
    [Separator]
    [SerializeField] bool updateCI;
    [SerializeField] bool updateCI2;

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        SetRealDateTime();
        SetTotalPlayTime();
        SetCurrency();

        if (updateCI)
        {
            AddCharacterUnit(_cUnit, 77);

            updateCI = false;
            return;
        }

        if (updateCI2)
        {
            SetCharacters();
            SetItems();

            updateCI2 = false;
            return;
        }
    }

    void SetRealDateTime()
    {
        System.DateTime myTime = System.DateTime.Now;
        GameData.dateTime = myTime.ToString();
        SaveLoad.SaveSystem.savedData.dateTime = GameData.dateTime;

        currentDateTime = GameData.dateTime;
    }

    void SetTotalPlayTime()
    {
        GameData.gameplayTime = FormatTime(this.TotalTime);
        SaveLoad.SaveSystem.savedData.gameplayTime = GameData.gameplayTime;
        currentGameplayTime = GameData.gameplayTime;
    }

    #region Total Play Time Function
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

    void SetCharacters()
    {
        listCharacters = GameData.listCharacters;
    }

    public void AddCharacterUnit(HBCharacterBase _characterUnit, int _level)
    {
        GameData.listCharacters.Add(new UserCharactersSlot(_characterUnit, _level));
    }

    void SetItems()
    {
        listItems = GameData.listItems;
    }

    void SetCurrency()
    {
        currency = $"${GameData.gameCurrency.ToString()}";
    }
}
