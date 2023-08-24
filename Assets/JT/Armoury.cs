using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Armoury : MonoBehaviour // ManagerUI
{
    public static Armoury Instance { get; private set; }

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    [ReadOnly] public GameController cController;
    public Transform parentArmouryList;
    public GameObject characterBoxUI;

    public GameObject equipMainCharSlot;
    public GameObject equipSubCharSlot;
    [ReadOnly] public List<GameObject> hbCharSlots;

    //-------------------------------------------------------------
    public UserCharactersSlot[] hbChars;
    public UserCharactersSlot equippedMain;
    public UserCharactersSlot equippedSub;
    //-------------------------------------------------------------

    //The char equip slot UI on the Armoury panel
    public ArmouryHoldSlot charHoldSlot;

    [Separator]
    [ReadOnly] public string charNameText;
    [ReadOnly] public string charLvText;
    [ReadOnly] public string charXPText;

    // Start is called before the first frame update
    void Start()
    {
        cController = FindObjectOfType<GameController>();

        RenderInventory();
        AssignSlotIndexes();
    }
    //------------------------------------------------------------- Equipping
    public void ArmouryToMain(int slotIndex)
    {
        //Cache the Inventory slot ItemData from InventoryManager
        UserCharactersSlot charToEquip = GameData.listCharSlot[slotIndex];

        //Change the Inventory Slot to the Hand's
        GameData.listCharSlot[slotIndex] = equippedMain;

        //Change the Hand's Slot to the Inventory Slot's
        equippedMain = charToEquip;

        //Update the changes to the UI
        Instance.RenderInventory();
    }

    public void MainToArmoury()
    {
        //Iterate through each inventory slot and find an empty slot
        for (int i = 0; i < GameData.listCharSlot.ToArray().Length; i++)
        {
            if (GameData.listCharSlot[i] == null)
            {
                //Send the equipped item over to its new slot
                GameData.listCharSlot[i] = equippedMain;
                //Remove the item from the hand
                equippedMain = null;
                break;
            }
        }

        //Update the changes to the UI
        Instance.RenderInventory();
    }

    public void ArmouryToSub(int slotIndex)
    {
        //Cache the Inventory slot ItemData from InventoryManager
        UserCharactersSlot charToEquip = GameData.listCharSlot[slotIndex];

        //Change the Inventory Slot to the Hand's
        GameData.listCharSlot[slotIndex] = equippedSub;

        //Change the Hand's Slot to the Inventory Slot's
        equippedSub = charToEquip;

        //Update the changes to the UI
        Instance.RenderInventory();
    }

    public void SubToArmoury()
    {
        //Iterate through each inventory slot and find an empty slot
        for (int i = 0; i < GameData.listCharSlot.ToArray().Length; i++)
        {
            if (GameData.listCharSlot[i] == null)
            {
                //Send the equipped item over to its new slot
                GameData.listCharSlot[i] = equippedSub;
                //Remove the item from the hand
                equippedSub = null;
                break;
            }
        }

        //Update the changes to the UI
        Instance.RenderInventory();
    }
    //-------------------------------------------------------------

    //Iterate through the slot UI elements and assign it its reference slot index
    public void AssignSlotIndexes()
    {
        for (int i = 0; i < hbChars.Length; i++)
        {
            hbCharSlots[i].GetComponent<ArmourySlot>().AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        //Get the character armoury from GameData
        UserCharactersSlot[] armouryCharSlots = GameData.listCharSlot.ToArray();
        RenderArmouryPanel(armouryCharSlots, hbCharSlots);

        //BattleUnitSlot equippedCharacter = new BattleUnitSlot 
        //{
        //    battleUnit = cController.bUnitSlot.battleUnit,
        //    subBattleUnit = cController.bUnitSlot.subBattleUnit
        //};

        ////Get Tool Equip Main from GameData Armoury
        //UserCharactersSlot equippedMainChar = GameData.bUnitSlot.battleUnit;

        ////Get Tool Equip Sub from GameData Armoury
        //UserCharactersSlot equippedSubChar = GameData.bUnitSlot.subBattleUnit;

        //equippedMain = GameData.bUnitSlot.battleUnit;
        //equippedSub = GameData.bUnitSlot.subBattleUnit;

        if (equippedMain != null)
        {
            equipMainCharSlot.GetComponent<ArmourySlot>().charSlot = equippedMain;
        }
        if (equippedSub != null)
        {
            equipSubCharSlot.GetComponent<ArmourySlot>().charSlot = equippedSub;
        }

    }
    void RenderArmouryPanel(UserCharactersSlot[] slots, List<GameObject> uiSlots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject assetUI = Instantiate(characterBoxUI);
            assetUI.GetComponent<ArmourySlot>().charSlot.characterBase = slots[i].characterBase;
            assetUI.GetComponent<ArmourySlot>().charSlot.level = slots[i].level;
            assetUI.GetComponent<ArmourySlot>().charSlot.currentXP = slots[i].currentXP;

            uiSlots.Add(assetUI);
            assetUI.transform.SetParent(parentArmouryList.transform);
        }
    }

    //Display Item info on the Item infobox
    public void DisplayItemInfo(UserCharactersSlot data)
    {
        charNameText = data?.characterBase?.Name ?? "None";
        charLvText = data?.level.ToString() ?? "0";
        charXPText = data?.currentXP.ToString() ?? "0";
    }
}
