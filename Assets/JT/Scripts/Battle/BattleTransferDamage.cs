using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransferDamage
{
    // Store the BattleUnit Data
    [ReadOnly] public int atk;
    [ReadOnly] public int spAtk;
    [ReadOnly] public int crit;
}

public class BattleTransferDamage : MonoBehaviour
{
    [LabelOverride("Transfer Damage")]
    [SerializeField] public TransferDamage tDamage;

    // Check if this is a Player or Enemy
    [HideInInspector] public bool isPlayer;
}