using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    public BulletBase Base { get; set; }

    public Bullet(BulletBase hbBase)
    {
        Base = hbBase;
    }

}
