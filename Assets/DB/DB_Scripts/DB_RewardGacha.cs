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

	public RewardType rewardType;

	[System.Serializable]
	public class RewardType
    {
		public HBCharacterBase charBase;		//This is James' scriptable object for characters
		public DB_ElementalData elemData;		//This will be the scriptable object for items
	}
}
