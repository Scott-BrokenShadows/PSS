using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HBCharacter
{
    public HBCharacterBase Base { get; set; }

    public HBCharacter(HBCharacterBase sBase)
    {
        Base = sBase;
    }
}
