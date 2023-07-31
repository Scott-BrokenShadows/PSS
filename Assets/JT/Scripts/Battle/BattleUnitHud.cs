using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitHud : MonoBehaviour
{
    [SerializeField] public HPBar hpBar;

    HBCharacter _hbCharacter;

    public void SetData(HBCharacter hbCharacter)
    {
        _hbCharacter = hbCharacter;
        hpBar.SetHP((float) hbCharacter.HP / hbCharacter.MaxHP);
    }

    public void UpdateHP()
    {
        hpBar.SetHP((float)_hbCharacter.HP / _hbCharacter.MaxHP);
    }
}
