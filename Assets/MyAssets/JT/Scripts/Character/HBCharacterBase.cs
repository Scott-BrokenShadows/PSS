using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region Custom Class
[System.Serializable]
public class DetectionRange
{
    #region Detection
    [Header("Detection Range")]
    public float detectionRadiusV = 10.0f; // V = view
    public float detectionRadiusS = 10.0f; // S = sound
    public float detectionAngle = 90.0f;
    [Range(0, 100)] public float detectionRangeHiddenV;
    [Range(0, 100)] public float detectionRangeHiddenS;
    public Color detectVColor = new Color(0.8f, 0f, 0f, 0.4f);
    public Color detectSColor = new Color(0f, 0f, 0f, 0.4f);
    public bool detectionGizmo;
    public bool hiddenRangeGizmo;

    [Header("<Player Range>")]
    public Color plyRadiusColor = new Color(0f, 50f, 90f, 0.3f);
    public float plyRadiusSize = 11f;
    public bool playerGizmo;

    [Header("<Attack Range>")]
    public Color atkRadiusColor = new Color(255f, 255f, 255f, 0.15f);
    public float atkRadiusSize = 5f;
    public bool attackGizmo;
    #endregion
}
#endregion

[CreateAssetMenu(fileName = "HBCharacterBase", menuName = "HBCharacter/Create New HBCharacter")]
public class HBCharacterBase : GatchaBase
{
    // Name, Description, Image, 3D Model, Type, Attribute, Stage, Status, Rate
    [SerializeField] new string name;
    [SerializeField] int dexNumber;
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
    [Min (0)]
    [SerializeField] int attack;
    [Min(0)]
    [SerializeField] int spAttack;
    [Min(0)]
    [SerializeField] int defence;
    [Min(0)]
    [SerializeField] int spDefence;
    [Min(0)]
    [SerializeField] int speed;

    [SerializeField] [ReadOnly] int totalStats;

    public string Name { get { return name; } }
    public int DexNumber { get { return dexNumber; } }
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
