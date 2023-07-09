using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_SystemGacha : MonoBehaviour
{
    public int rollAttempts;     //How many pulls are conducted in one go.
    public int highRare;         //Part of Pity. Increases with each non-Rarity 3 pull.
    public int pityLimit = 50;   //Guarantees a Rarity 3 after this many pulls without a Rarity 3.
    public float dRoll;          //The numbered result of a pull, between 1 & 100.

    public bool rareBonus;       //Guarantees at least a Rarity 2 when true.

    //These are the % chance of obtaining these Rarities.
    public float rareTier1 = 75f;
    public float rareTier2 = 20f;
    public float rareTier3 = 5f;

    //[SerializeField] private DB_RewardRate gacha;
    //[SerializeField] Transform parent, pos;
    //[SerializeField] private GameObject itemRewardGO;
    //GameObject itemReward;
    //DB_Reward reward;

    //public List<DB_RewardGacha> rarity1List = new List<DB_RewardGacha>();
    //public List<DB_RewardGacha> rarity2List = new List<DB_RewardGacha>();
    //public List<DB_RewardGacha> rarity3List = new List<DB_RewardGacha>();

    public List<DB_RewardRate> rarity1List;
    public List<DB_RewardRate> rarity2List;
    public List<DB_RewardRate> rarity3List;

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
            Debug.Log("******************");
            Debug.Log("Pulls Complete!");
            //Will send to a pull summary screen
        }
        else
        {
            dRoll = float.Parse((Random.Range(0.0f, 100.0f)).ToString("F2"));   //'Fx', where 'x' = how many decimal points.
            rollAttempts -= 1;
            Debug.Log("******************");
            RarityCheck();      //Determines what Rarity was obtained.
            RollAttempt();      //Loops this system until the attempts reachs 0.
        }
    }

    public void RarityCheck()
    {
        if((highRare == pityLimit) || (dRoll <= 5))
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
        Debug.Log($"Rolled {dRoll}. RARITY 3 ITEM OBTAINED! PITY RESET!");
        GetRandomReward(rarity3List);
        //Will determine which Rarity 3 item was obtained.
    }

    public void RareTier2Get()
    {
        highRare += 1;
        rareBonus = false;
        Debug.Log($"Rolled {dRoll}. Rarity 2 Item Obtained! Pity Score: {highRare}");
        GetRandomReward(rarity2List);
        //Will determine which Rarity 2 item was obtained.
    }

    public void RareTier1Get()
    {
        highRare += 1;
        Debug.Log($"Rolled {dRoll}. Rarity 1 Item Obtained! Pity Score: {highRare}");
        GetRandomReward(rarity1List);
        //Will determine which Rarity 1 item was obtained.
    }

    public void GetRandomReward(List<DB_RewardRate> thisList)
    {
        var totalWeight = 0.0f;
        foreach (var entry in thisList)
        {
            totalWeight += entry.rate;
        }
        var rndWeightValue = float.Parse((Random.Range(0.0f, totalWeight + 1.0f)).ToString("F2"));

        var processedWeight = 0.0f;
        foreach (var entry in thisList)
        {
            processedWeight += entry.rate;
            if(rndWeightValue <= processedWeight)
            {
                Debug.Log($"Obtained {entry.rateName}!");
                break;
            }
        }
    }
}
