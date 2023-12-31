using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillBase", menuName = "HBCharacter/Create New Skill")]
public class SkillBase : ScriptableObject
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
    // Timer
    [SerializeField] float skillReload;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Sprite IconSprite { get { return iconSprite; } }
    public GameObject Asset { get { return asset; } }
}
