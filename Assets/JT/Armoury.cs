using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Armoury : MonoBehaviour
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

    [ReadOnly] public string charNameText;
    [ReadOnly] public string charLvText;
    [ReadOnly] public string charXPText;

    // Start is called before the first frame update
    void Start()
    {
        cController = FindObjectOfType<GameController>();

        RenderInventory();
    }

    public void RenderInventory()
    {
        //Get the character armoury from GameData
        UserCharactersSlot[] armouryCharSlots = GameData.listCharSlot.ToArray();
        RenderArmouryPanel(armouryCharSlots, hbCharSlots);

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

        //foreach (var var in cController.listCharSlot)
        //{
        //    GameObject assetUI = Instantiate(characterBoxUI);
        //    assetUI.GetComponent<ArmourySlot>().charSlot.characterBase = var.characterBase;
        //    assetUI.GetComponent<ArmourySlot>().charSlot.level = var.level;
        //    assetUI.GetComponent<ArmourySlot>().charSlot.currentXP = var.currentXP;
        //    charListUI.Add(assetUI);

        //    assetUI.transform.SetParent(parentArmouryList.transform);
        //}
    }

    //Display Item info on the Item infobox
    public void DisplayItemInfo(UserCharactersSlot data)
    {
        charNameText = data?.characterBase?.Name ?? "None";
        charLvText = data?.level.ToString() ?? "0";
        charXPText = data?.currentXP.ToString() ?? "0";

        ////If data is null, reset
        //if (data == null)
        //{
        //    charNameText = "None";
        //    charLvText = "0";
        //    charXPText = "0";

        //    return;
        //}

        //charNameText = data.characterBase.Name;
        //charLvText = data.level.ToString();
        //charXPText = data.currentXP.ToString();
    }
}
