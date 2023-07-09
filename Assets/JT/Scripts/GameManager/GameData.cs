using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserCharacters
{
    public int id; //Example: item001, item002, item003
    public int level; //What level is the Character
}

[System.Serializable]
public class Item
{
    public int id; //Example: item001, item002, item003
    public int quantity; //How much item you have
}

public static class GameData //Store the Data here
{
    public static string dateTime;
    public static string gameplayTime;
    public static int gameCurrency;
    public static List<UserCharacters> listCharacters;
    public static List<UserCharacters> listPartyBattleUnits;
    public static List<Item> listItems;
}
