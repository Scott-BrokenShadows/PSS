using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HBCharacter
{
    public HBCharacterBase Base { get; set; }

    public int Level { get; set; }

    public int HP { get; set; }
    public Skill Skills { get; set; }

    public HBCharacter(HBCharacterBase hbBase, int hbLevel)
    {
        Base = hbBase;
        Level = hbLevel;
        HP = MaxHP;
    }

    #region Stats Calculation
    // Those that return status according to the level: Property (+ processing can be added)

    // Attack------------------------------------------------------------------------
    public int Attack { get { return CalculateAttack(); } }
    int CalculateAttack() { return Mathf.FloorToInt(((Base.Attack * Level) / 100f) + 5); }
    // SPAttack------------------------------------------------------------------------
    public int SpAttack { get { return CalculateSpAttack(); } }
    int CalculateSpAttack() { return Mathf.FloorToInt(((Base.SpAttack * Level) / 100f) + 5); }
    // Defense------------------------------------------------------------------------
    public int Defense { get { return CalculateDefense(); } }
    int CalculateDefense() { return Mathf.FloorToInt(((Base.Defence * Level) / 100f) + 5); }
    // SPDefense------------------------------------------------------------------------
    public int SpDefense { get { return CalculateSpDefense(); } }
    int CalculateSpDefense() { return Mathf.FloorToInt(((Base.SpDefence * Level) / 100f) + 5); }
    // Speed------------------------------------------------------------------------
    public int Speed { get { return CalculateSpeed(); } }
    int CalculateSpeed() { return Mathf.FloorToInt(((Base.Speed * Level) / 100f) + 5); }
    // HP------------------------------------------------------------------------
    public int MaxHP { get { return CalculateMaxHP(); } }
    int CalculateMaxHP() { return Mathf.FloorToInt(((Base.MaxHP * Level) / 100f) + 10); }
    // Take Damage------------------------------------------------------------------------
    #region Take Damage Calculation
    public DamageDetails TakeDamage(HBCharacter attacker) // Attack
    {
        // Critical
        float critical = 1f; // Normal damage <no critical>
        if (Random.value * 100f <= attacker.Base.Critical) // HBCharacter Critical% chance critical 
        {
            // Critical Damage
            critical = 1.5f;
        }
        // Compability
        float type = ElementChart.GetEffectiveness(attacker.Base.Elements, this.Base.Elements);

        float modifiers = Random.Range(0.85f, 1.05f) * type * critical;

        // Attack Damage
        int damageBasic = 0;
        float basic = (float)attacker.Attack * 50 / (float)this.Defense;
        damageBasic = Mathf.FloorToInt(basic * modifiers);

        int damage = damageBasic;

        int damageD = Mathf.Clamp(damage, 0, 999);

        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            Critical = critical,
            Damage = damageD,
            Effectiveness = type
        };

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;

    }
    #endregion

    #endregion
}

public class DamageDetails
{
    public bool Fainted { get; set; } // Whether it is impossible to fight
    public float Critical { get; set; }
    public float Damage { get; set; }
    public float Effectiveness { get; set; }
}
