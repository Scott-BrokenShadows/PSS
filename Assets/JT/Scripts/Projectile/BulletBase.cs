using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletBase", menuName = "HBCharacter/Create New Bullet")]
public class BulletBase : ScriptableObject
{
    // Name, Description, Image
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;

    [Separator()]
    // Image
    [SerializeField] public Sprite iconSprite;

    // 3D Model
    [SerializeField] GameObject asset;

    [Separator()]
    // Type of Bullet
    [LabelOverride("Type")]
    [SerializeField] BulletType bulletType;
    // Range
    [LabelOverride("Range")]
    [SerializeField] float bulletRange;
    // Count
    [LabelOverride("Count")]
    [SerializeField] int bulletCount;
    // Timer
    [LabelOverride("Fire Rate")]
    [SerializeField] float bulletReload;
    // Speed
    [LabelOverride("Speed")]
    [SerializeField] float bulletSpeed;
    // Aim at Andy
    [LabelOverride("Aim At Player")]
    [SerializeField] bool aimAtPlayer;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Sprite IconSprite { get { return iconSprite; } }
    public GameObject Asset { get { return asset; } }

    public float BulletRange { get { return bulletRange; } }
    public int BulletCount { get { return bulletCount; } }
    public float BulletReload { get { return bulletReload; } }
    public float BulletSpeed { get { return bulletSpeed; } }
    public bool AimAtPlayer { get { return aimAtPlayer; } }

    public BulletType BulletType { get { return bulletType; } }
}

public enum BulletType
{
    SingleShot,
    MultiLaneShot,
    MultiSpreadShot
}
