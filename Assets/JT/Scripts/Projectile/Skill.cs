using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public SkillBase Base { get; set; }

    public Skill(SkillBase hbBase)
    {
        Base = hbBase;
    }
}
