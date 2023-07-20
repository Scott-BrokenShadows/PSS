using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HBCharacterBase", menuName = "HBCharacter/Create New HBCharacter")]
public class HBCharacterBase : ScriptableObject
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
    [SerializeField] GameObject asset;

    [Separator()]
    // Base Stats
    [Header("Base stats")]
    [Range (0, 999)]
    [SerializeField] int attack;
    [Range(0, 999)]
    [SerializeField] int spAttack;
    [Range(0, 999)]
    [SerializeField] int defence;
    [Range(0, 999)]
    [SerializeField] int spDefence;
    [Range(0, 999)]
    [SerializeField] int speed;

    [SerializeField] [ReadOnly] int totalStats;

    public string Name { get { return name; } }
    public int ID { get { return id; } }
    public string Description { get { return description; } }
    public Sprite IconSprite { get { return iconSprite; } }
    public GameObject Asset { get { return asset; } }
    public int Attack { get { return attack; } }
    public int Speed { get { return speed; } }
}

public enum SnapmonPersonality
{
    Neutral,
    Coward,
    Hostile
}

public enum SnapmonAttackRange
{
    CloseRange,
    MidRange,
    FarRange
}

public enum SnapmonLocationType
{
    Ground,
    Water,
    Air
}
