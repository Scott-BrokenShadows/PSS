using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    HBCharacter _hbCharacter;

    public void SetData(HBCharacter hbCharacter)
    {
        _hbCharacter = hbCharacter;
        nameText.text = hbCharacter.Base.Name;
        levelText.text = "Lv."+hbCharacter.Level.ToString();
        hpBar.SetHP((float) hbCharacter.HP / hbCharacter.MaxHP);
    }

    public void UpdateHP()
    {
        hpBar.SetHP((float)_hbCharacter.HP / _hbCharacter.MaxHP);
    }
}
