using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new item", menuName = "Elemental Item")]
public class DB_ElementalData : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
}
