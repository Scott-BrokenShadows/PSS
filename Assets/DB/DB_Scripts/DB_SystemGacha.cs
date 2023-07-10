using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class GatchaSlot
{
    //public int id; //Example: item001, item002, item003
    public DB_RewardGacha itemBase;

    public GatchaSlot(DB_RewardGacha _itemUnit)
    {
        itemBase = _itemUnit;
    }
}

public class DB_SystemGacha : MonoBehaviour
{
    public int rollAttempts;     //How many pulls are conducted in one go.
    public int highRare;         //Part of Pity. Increases with each non-Rarity 3 pull.
    public int pityLimit = 50;   //Guarantees a Rarity 3 after this many pulls without a Rarity 3.
    public float dRoll;          //The numbered result of a pull, between 1 & 100.

    public bool rareBonus;       //Guarantees at least a Rarity 2 when true.

    public GameObject x10Grid;
    //public GameObject x1Grid;

    //These are the % chance of obtaining these Rarities.
    public float rareTier1 = 75f;
    public float rareTier2 = 20f;
    public float rareTier3 = 5f;

    //This list is where each individual item as Scriptable Objects are kept.
    public List<DB_RewardGacha> rarity1ItemList;
    public List<DB_RewardGacha> rarity2ItemList;
    public List<DB_RewardGacha> rarity3ItemList;

    [Separator]
    [Header("Inventory")]
    [ReadOnly] public List<GatchaSlot> itemsObtained;

    public void RequestRollX1()
    {
        itemsObtained.Clear();

        foreach (Transform child in x10Grid.transform)
        {
            child.GetComponent<Image>().sprite = null;
        }

        rollAttempts = 1;
        rareBonus = false;
        RollAttempt();
    }

    public void RequestRollX10()
    {
        itemsObtained.Clear();

        foreach (Transform child in x10Grid.transform)
        {
            child.GetComponent<Image>().sprite = null;
        }

        rollAttempts = 10;
        rareBonus = true;
        RollAttempt();
    }

    public void RollAttempt()
    {
        if (rollAttempts == 0)
        {
            Debug.Log("******************");
            Debug.Log("Pulls Complete!");
            for (int i = 0; i < itemsObtained.Count; i++)
            {
                x10Grid.transform.GetChild(i).GetComponent<Image>().sprite = itemsObtained[i].itemBase.itemImage;
            }
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
        if((highRare == pityLimit) || (dRoll <= rareTier3))
        {
        RareTier3Get();
        }
        else if ((dRoll >= rareTier2) && (rareBonus == false))
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
        GetRandomReward(rarity3ItemList);
        //Will determine which Rarity 3 item was obtained.
    }

    public void RareTier2Get()
    {
        highRare += 1;
        rareBonus = false;
        Debug.Log($"Rolled {dRoll}. Rarity 2 Item Obtained! Pity Score: {highRare}");
        GetRandomReward(rarity2ItemList);
        //Will determine which Rarity 2 item was obtained.
    }

    public void RareTier1Get()
    {
        highRare += 1;
        Debug.Log($"Rolled {dRoll}. Rarity 1 Item Obtained! Pity Score: {highRare}");
        GetRandomReward(rarity1ItemList);
        //Will determine which Rarity 1 item was obtained.
    }

    public void GetRandomReward(List<DB_RewardGacha> thisList)
    {
        var totalWeight = 0.0f;
        foreach (var entry in thisList)
        {
            totalWeight += entry.dropChance;
        }
        var rndWeightValue = float.Parse((Random.Range(0.0f, totalWeight)).ToString("F2"));

        var processedWeight = 0.0f;
        foreach (var entry in thisList)
        {
            processedWeight += entry.dropChance;
            if(rndWeightValue <= processedWeight)
            {
                Debug.Log($"Obtained {entry.itemName}!");
                //Add Item to the Grid Array

                if (itemsObtained.Count < x10Grid.transform.childCount)
                {
                    AddItem(entry);
                }

                if (entry.isCharacter == true)
                {
                    Debug.Log($"Splash Animation of {entry.itemName} happens here!");
                }
                break;
            }
        }
    }

    void Update()
    {


        #region Error
        //for (int i = 0; i < x10Grid.transform.childCount - 1; i++)
        //{
        //    if (itemsObtained[i] != null)
        //    {
        //        x10Grid.transform.GetChild(i).GetComponent<Image>().sprite = itemsObtained[i].itemBase.itemImage;
        //    }
        //}
        #endregion
    }

    void AddItem(DB_RewardGacha _DB_RewardGacha)
    {
        itemsObtained.Add(new GatchaSlot(_DB_RewardGacha));
    }
}
