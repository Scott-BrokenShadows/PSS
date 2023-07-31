using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransferDamage
{
    // Store the BattleUnit Data
    [LabelOverride("Element")]
    [ReadOnly] public Elements elements;
    [LabelOverride("Physical Attack")]
    [ReadOnly] public int atk;
    [LabelOverride("Magic Attack")]
    [ReadOnly] public int spAtk;
    [LabelOverride("Critical")]
    [ReadOnly] public int crit;
}

public class BattleTransferDamage : MonoBehaviour
{
    [SerializeField] public TransferDamage transferDamage;

    // Check if this is a Player or Enemy
    public bool isPlayer;
}