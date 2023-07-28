using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSkill : MonoBehaviour
{
    [LabelOverride("Skill Base")]
    [SerializeField] public SkillBase _base;
    [LabelOverride("Unit Base")]
    [SerializeField] public BattleUnit bUnit;

    // Check if this is a Player or Enemy
    [HideInInspector] public bool isPlayer;

    // Store the BattleUnit Data
    [ReadOnly] public int atk;
    [ReadOnly] public int spAtk;
    [ReadOnly] public int crit;

    // Keep Data this gameobject
    GameObject myGameObject;

    void Start()
    {
        // Set up the data
        if (_base) { SetUp(); }
    }

    void SetUp()
    {
        #region Instantiate the Asset and Name them
        this.gameObject.name = (_base.Name != "") ? _base.Name + ((isPlayer) ? "(Player)" : "(Enemy)") : "Default";
        myGameObject = _base.Asset;
        GameObject asset = Instantiate(_base.Asset, transform);
        asset.transform.position = transform.position;
        asset.transform.localRotation = Quaternion.Euler(0, ((isPlayer) ? 0 : 180), 0);
        asset.name = _base.Name + "Model";
        #endregion
    }
}
