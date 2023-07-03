using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_GachaSystem : MonoBehaviour
{
    public int rollAttempts;
    public int highRare;
    public float dRoll;

    public float rareTier1 = 75f;
    public float rareTier2 = 20f;
    public float rareTier3 = 5f;

    public bool rareBonus;

    public void RequestRollX1()
    {
        rollAttempts = 1;
        rareBonus = false;
        RollAttempt();
    }

    public void RequestRollX10()
    {
        rollAttempts = 10;
        rareBonus = true;
        RollAttempt();
    }

    public void RollAttempt()
    {
        if(rollAttempts == 0)
        {
            Debug.Log("Pulls Complete!");
        }
        else
        {
            dRoll = Random.Range(0.0f, 100.0f);
            Debug.Log(dRoll + " was rolled");
            RarityCheck();
            rollAttempts -= 1;
            RollAttempt();
        }
    }

    //for rounding decimal: Mathf.Round()
    public void RarityCheck()
    {
        //if((highRare == Mathf.Round(50 * 100) / 100) || (dRoll <= Mathf.Round(5 * 100) / 100))
        if((highRare == 50) || (dRoll <= 5))
        {
        RareTier3Get();
        }
        else if ((dRoll >= 25) && (rareBonus == false))
        {
            RareTier1Get();
        }
        else
        {
            RareTier2Get();
        }
    }

    public void RareTier3Get()
    {
        highRare = 0;
        rareBonus = false;
        Debug.Log($"RARITY3 ITEM OBTAINED! PITY RESET!");
    }

    public void RareTier2Get()
    {
        highRare += 1;
        rareBonus = false;
        Debug.Log($"Rarity 2 Item Obtained! Pity Score: {highRare}");
    }

    public void RareTier1Get()
    {
        highRare += 1;
        Debug.Log($"Rarity 1 Item Obtained! Pity Score: {highRare}");
    }
}
