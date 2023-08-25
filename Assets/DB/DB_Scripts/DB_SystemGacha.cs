using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

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

    public TMP_Text confirmText;
    public int currencyGacha = 10000;
    public TMP_Text currencyText;
    public int singleRollCost = 180;
    public TMP_Text singleRollText;
    public int tenRollCost = 1800;
    public TMP_Text tenRollText;
    private int currentRollCost;
    public TMP_Text pityText;

    public GameObject grid;
    public GameObject gridBG;
    public GameObject xRollButtonGroup;
    public GameObject rollConfirmButton;
    public GameObject rollBackButton;
    public GameObject shopConfirmButton;
    public GameObject continueButton;
    public Sprite nullImage;

    //These are the % chance of obtaining these Rarities.
    public float rareTier1 = 75f;
    public float rareTier2 = 20f;
    public float rareTier3 = 5f;

    //This list is where each individual item as Scriptable Objects are kept.
    public List<DB_RewardGacha> rarity1ItemList;
    public List<DB_RewardGacha> rarity2ItemList;
    public List<DB_RewardGacha> rarity3ItemList;

    public GameController gameController;

    [Separator]
    [Header("Inventory")]
    [ReadOnly] public List<GatchaSlot> itemsObtained;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        singleRollText.text = $"x{singleRollCost}";
        tenRollText.text = $"x{tenRollCost}";
        pityText.text = $"Rarity 3 Guaranteed in {pityLimit - highRare} Attempts!";
        MenuReset();
    }

    //Sets Roll Buttons active, and disables the Grid and continue Button.
    public void MenuReset()
    {
        currencyText.text = $"{currencyGacha}";
        pityText.text = $"Rarity 3 Guaranteed in {pityLimit - highRare} Attempts!";
        rollAttempts = 0;
        currentRollCost = 0;
        rareBonus = false;
        grid.SetActive(false);
        gridBG.SetActive(false);
        xRollButtonGroup.SetActive(true);
        rollConfirmButton.SetActive(false);
        rollBackButton.SetActive(false);
        shopConfirmButton.SetActive(false);
        continueButton.SetActive(false);
        confirmText.text = null;

        foreach (Transform child in grid.transform)
        {
            child.GetComponent<Image>().sprite = nullImage;
        }

    }

    public void RequestRollX1()
    {
        if (currencyGacha >= singleRollCost)
        {
            currentRollCost = singleRollCost;
            rollAttempts = 1;
            rareBonus = false;
            RollStandby();
        }
        else
        {
            RollInsufficient();
        }
    }

    public void RequestRollX10()
    {
        if (currencyGacha >= tenRollCost)
        {
            currentRollCost = tenRollCost;
            rollAttempts = 10;
            rareBonus = true;
            RollStandby();
        }
        else
        {
            RollInsufficient();
        }
    }

    public void RollInsufficient()
    {
        itemsObtained.Clear();
        grid.SetActive(true);
        gridBG.SetActive(true);
        xRollButtonGroup.SetActive(false);
        rollBackButton.SetActive(true);
        shopConfirmButton.SetActive(true);
        confirmText.text = $"Insufficient Gems. Add funds?";
    }

    public void RollStandby()
    {
        currencyText.text = $"{currencyGacha}";
        itemsObtained.Clear();
        grid.SetActive(true);
        gridBG.SetActive(true);
        xRollButtonGroup.SetActive(false);
        rollConfirmButton.SetActive(true);
        rollBackButton.SetActive(true);
        confirmText.text = $"Spend {currentRollCost} for {rollAttempts} attempt(s)?";
    }

    public void RollConfirm()
    {
        rollConfirmButton.SetActive(false);
        rollBackButton.SetActive(false);
        confirmText.text = null;
        currencyGacha -= currentRollCost;
        currencyText.text = $"{currencyGacha}";
        RollAttempt();
    }

    public void RollAttempt()
    {
        if (rollAttempts == 0)
        {
            Debug.Log("******************");
            Debug.Log("Pulls Complete!");
            StartCoroutine(GachaTimer());
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

    //Timer for displaying the Gacha results one at a time.
    IEnumerator GachaTimer()
    {
        for (int i = 0; i < itemsObtained.Count; i++)
        {
            grid.transform.GetChild(i).GetComponent<Image>().sprite = itemsObtained[i].itemBase.itemImage;
            yield return new WaitForSeconds(0.2f);
        }
        continueButton.SetActive(true);
    }

    //Confirms the Rarity of the Item pulled based on the roll.
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
        foreach (var attempt in thisList)
        {
            totalWeight += attempt.dropChance;
        }
        var dRollWeight = float.Parse((Random.Range(0.0f, totalWeight)).ToString("F2"));

        var maxWeight = 0.0f;
        foreach (var attempt in thisList)
        {
            maxWeight += attempt.dropChance;
            if(dRollWeight <= maxWeight)
            {
                Debug.Log($"Obtained {attempt.itemName}!");
                
                //Add this reward item to inventory

                if (itemsObtained.Count < grid.transform.childCount)
                {
                    AddItem(attempt);
                }

                if (attempt.isCharacter == true)
                {
                    Debug.Log($"Splash Animation of {attempt.itemName} happens here!");

                    gameController.AddCharacterUnit(attempt.rewardType.charBase, 1, 0);

                    gameController.SetCharacters();

                }

                if (attempt.isCharacter == false)
                { 
                
                }

                break;
            }
        }
    }

    #region Error
    void Update()
    {
        //for (int i = 0; i < x10Grid.transform.childCount - 1; i++)
        //{
        //    if (itemsObtained[i] != null)
        //    {
        //        x10Grid.transform.GetChild(i).GetComponent<Image>().sprite = itemsObtained[i].itemBase.itemImage;
        //    }
        //}
    }
    #endregion

    public void AddFunds()
    {
        currencyGacha += 10000;
        MenuReset();
    }

    void AddItem(DB_RewardGacha _DB_RewardGacha)
    {
        itemsObtained.Add(new GatchaSlot(_DB_RewardGacha));
    }
}
