using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(HBCharacter hbCharacter)
    {
        nameText.text = hbCharacter.Base.Name;
        levelText.text = hbCharacter.Level.ToString();
        hpBar.SetHP((float) hbCharacter.HP / hbCharacter.MaxHP);
    }
}
