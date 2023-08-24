using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourySlotData
{
    // Get the data from the database
    public HBCharacterBase _characterData;
    public int _level;
    public int _currentXP;


    //Class Constructor
    public ArmourySlotData(HBCharacterBase _characterUnit, int _level, int _currentXP)
    {
        this._characterData = _characterUnit;
        this._level = _level;
        this._currentXP = _currentXP;
        ValidateEmpty();
    }

    public void Remove()
    {
        ValidateEmpty();
    }

    private void ValidateEmpty()
    {
        if (_characterData == null)
        {
            Empty();
        }
    }

    //Empty out the Armoury Slot
    public void Empty()
    {
        _characterData = null;
        _level = 0;
        _currentXP = 0;
    }
}
