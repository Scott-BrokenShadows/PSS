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
    [SerializeField] UnitType unitType;
    [SerializeField] Elements elements;
    [SerializeField] DefaultBulletType defaultBulletType;

    [Separator()]
    // Base Stats
    [Header("Base stats")]
    [Range(0, 9999)]
    [SerializeField] int maxHP;
    [Range(0, 999)]
    [SerializeField] int attack;
    [Range(0, 999)]
    [SerializeField] int spAttack;
    [Range(0, 999)]
    [SerializeField] int defence;
    [Range(0, 999)]
    [SerializeField] int spDefence;
    [Range(0, 999)]
    [SerializeField] int speed;

    // Total Calculation of total base stats not include hidden
    [SerializeField] [ReadOnly] int totalStats;

    // Hidden Stats
    [Header("Hidden stats")]
    [Range(0,100)]
    [SerializeField] int critical;

    private void OnValidate()
    {
        totalStats = attack + spAttack + defence + spDefence + speed;
    }

    [Separator()]
    [SerializeField] SkillBase unitSkill;

    #region Get and Set Value
    public string Name { get { return name; } }
    public int ID { get { return id; } }
    public string Description { get { return description; } }
    public Sprite IconSprite { get { return iconSprite; } }
    public GameObject Asset { get { return asset; } }

    public UnitType UnitType { get { return unitType; } }
    public Elements Elements { get { return elements; } }
    public DefaultBulletType DefaultBulletType { get { return defaultBulletType; } }

    public int MaxHP { get { return maxHP; } }

    public int Attack { get { return attack; } }
    public int SpAttack { get { return spAttack; } }
    public int Defence { get { return defence; } }
    public int SpDefence { get { return spDefence; } }
    public int Speed { get { return speed; } }
    public int Critical { get { return critical; } }

    public SkillBase UnitSkill { get { return unitSkill; } }
    #endregion
}

public enum UnitType
{
    Character,
    Minion,
    Boss
}

public enum Elements
{ 
    Fire,
    Water,
    Nature,
    Magic,
    Machine
}

public enum DefaultBulletType
{ 
    SingleShot,
    MultiLaneShot,
    MultiSpreadShot
}
