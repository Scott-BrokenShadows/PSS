using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserCharactersSlot
{
    //public int id; //Example: item001, item002, item003
    public HBCharacterBase characterBase;
    public int level; // What level is the Character

    // Experice Point for each Character
    public int currentXP;

    public UserCharactersSlot(HBCharacterBase _characterUnit, int _level, int _currentXP)
    {
        //id = _characterUnit.ID;
        characterBase = _characterUnit;
        level = _level;
        currentXP = _currentXP;
        ValidateEmpty();
    }

    public void Remove()
    {
        ValidateEmpty();
    }

    private void ValidateEmpty()
    {
        if (characterBase == null)
        {
            Empty();
        }
    }

    //Empty out the Armoury Slot
    public void Empty()
    {
        characterBase = null;
        level = 0;
        currentXP = 0;
    }
}

[System.Serializable]
public class BattleUnitSlot
{
    public UserCharactersSlot battleUnit;
    public UserCharactersSlot subBattleUnit;
}

[System.Serializable]
public class ItemSlot
{
    public int id; //Example: item001, item002, item003
    public int quantity; //How much item you have
}

public static class GameData //Store the Data here
{
    public static string dateTime;
    public static string gameplayTime;
    public static int gameCurrency;
    public static List<UserCharactersSlot> listCharSlot = new List<UserCharactersSlot>();
    public static BattleUnitSlot bUnitSlot; // bUnitSlot is "Battle Unit Slot"
    public static List<ItemSlot> listItems = new List<ItemSlot>();
}
