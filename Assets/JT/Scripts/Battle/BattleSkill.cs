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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
