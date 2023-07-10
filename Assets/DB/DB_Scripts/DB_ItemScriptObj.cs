using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new item", menuName = "Gacha Item")]
public class DB_ItemScriptObj : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
}
