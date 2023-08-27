using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserCharactersSlot
{
    //public int id; //Example: item001, item002, item003
    public HBCharacterBase characterBase;
    public int level; // What level is the Character
    public int currentXP; // Experice Point for each Character

    public UserCharactersSlot(HBCharacterBase _characterUnit, int _level, int _currentXP)
    {
        //id = _characterUnit.ID;
        characterBase = _characterUnit;
        level = _level;
        currentXP = _currentXP;
    }
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
