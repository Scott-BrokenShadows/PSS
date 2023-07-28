using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    // Name, Description, Image, 3D Model, Type, Attribute, Stage, Status, Rate
    [SerializeField] new string name;
    [SerializeField] int id;
    [TextArea]
    [SerializeField] string description;

    [Separator()]
    // Image
    [SerializeField] public Sprite iconSprite;

    // 3D Model
    //[SerializeField] GameObject asset;

    public string Name { get { return name; } }
    public int ID { get { return id; } }
    public string Description { get { return description; } }
    public Sprite IconSprite { get { return iconSprite; } }
    //public GameObject Asset { get { return asset; } }
}
