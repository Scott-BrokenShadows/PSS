using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Inspector------------------------------------------------------------------------

    #region Inspector
    public UserCharactersSlot _cUnit;
    [Separator]
    [HideInInspector] [SerializeField] string currentDateTime;
    [HideInInspector] [SerializeField] string currentGameplayTime;
    [HideInInspector] [SerializeField] string currency;
    [Separator]
    [ReadOnly] public List<UserCharactersSlot> listCharSlot = new List<UserCharactersSlot>();
    [ReadOnly] public BattleUnitSlot bUnitSlot;
    [ReadOnly] public List<ItemSlot> listItems;
    //[Separator]
    [SerializeField] bool updateCI;
    //[SerializeField] bool updateCI2;
    #endregion

    // Start & Update------------------------------------------------------------------------

    #region Start & Update
    void Start()
    {
        updateCI = true;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        //SetRealDateTime();
        //SetTotalPlayTime();
        //SetCurrency();

        TestingButton();
    }
    #endregion

    // This is testing the function is working------------------------------------------------------------------------

    #region Testing
    void TestingButton()
    {
        if (updateCI)
        {
            AddCharacterUnit(_cUnit.characterBase, _cUnit.level, _cUnit.currentXP);
            SetCharacters();

            SetItems();

            updateCI = false;
            return;
        }
    }
    #endregion

    // Setting Data to GameData------------------------------------------------------------------------

    #region Set Time Function
    void SetRealDateTime()
    {
        System.DateTime myTime = System.DateTime.Now;
        GameData.dateTime = myTime.ToString();
        //SaveLoad.SaveSystem.savedData.dateTime = GameData.dateTime;

        currentDateTime = GameData.dateTime;
    }

    void SetTotalPlayTime()
    {
        GameData.gameplayTime = FormatTime(this.TotalTime);
        //SaveLoad.SaveSystem.savedData.gameplayTime = GameData.gameplayTime;
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

    #endregion

    #region Set Characters

    #region Old Just ignore this
    //void SetCharacters()
    //{
    //    listCharacters = GameData.listCharacters;
    //    SaveLoad.SaveSystem.savedData.listCharacters = GameData.listCharacters;
    //}

    //public void AddCharacterUnit(HBCharacterBase _characterUnit, int _level)
    //{
    //    GameData.listCharacters.Add(new UserCharactersSlot(_characterUnit, _level));
    //}
    #endregion

    public void SetCharacters()
    {
        bUnitSlot = GameData.bUnitSlot;
        listCharSlot = GameData.listCharSlot;
    }

    public void AddBattleCharSlot(HBCharacterBase _characterUnit, int _level, int _currentXP)
    {
        GameData.bUnitSlot.battleUnit = new UserCharactersSlot(_characterUnit, _level, _currentXP);
    }

    public void AddBattleSubCharSlot(HBCharacterBase _characterUnit, int _level, int _currentXP)
    {
        GameData.bUnitSlot.subBattleUnit = new UserCharactersSlot(_characterUnit, _level, _currentXP);
    }

    public void AddCharacterUnit(HBCharacterBase _characterUnit, int _level, int _currentXP)
    {
        GameData.listCharSlot.Add(new UserCharactersSlot(_characterUnit, _level, _currentXP));
    }


    #endregion

    #region Set Items
    void SetItems()
    {
        listItems = GameData.listItems;
    }
    #endregion

    #region Set Currency
    void SetCurrency()
    {
        currency = $"${GameData.gameCurrency.ToString()}";
    }
    #endregion
}
