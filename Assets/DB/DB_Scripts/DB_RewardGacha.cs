using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new reward", menuName = "Gacha Item")]
public class DB_RewardGacha : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public float dropChance;
    public bool isCharacter = false;

	//public HBCharacterBase charBase;
	//public DB_ItemScriptObj itemBase;

	[SerializeField] RewardType rewardType;

	[System.Serializable]
	public class RewardType
    {
		public HBCharacterBase charBase;
		public DB_ElementalData elemData;
	}
}
