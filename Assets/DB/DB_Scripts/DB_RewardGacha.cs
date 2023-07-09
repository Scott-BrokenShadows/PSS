using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new reward", menuName = "Gacha Item")]
public class DB_RewardGacha : ScriptableObject
{
    public Sprite image;
    public string itemName;
    public float dropChance;
}
