using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armoury : MonoBehaviour
{
    [ReadOnly] public GameController cController;
    public Transform parentArmouryList;
    public GameObject characterBoxUI;

    public GameObject mainCharacter;
    public GameObject subCharacter;
    [ReadOnly] public List<GameObject> charListUI;

    public UserCharactersSlot[] hbCharSlots;
    public UserCharactersSlot equipMainCharSlot = null;
    public UserCharactersSlot equipSubCharSlot = null;

    public bool callOnce;

    // Start is called before the first frame update
    void Start()
    {
        cController = FindObjectOfType<GameController>();

        foreach (var var in cController.listCharSlot)
        {
            GameObject assetUI = Instantiate(characterBoxUI);
            charListUI.Add(assetUI);
            //assetUI.GetComponent<ArmouryCharacterBoxUI>().charName.text = var.characterBase.Name;
            assetUI.transform.SetParent(parentArmouryList.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (callOnce)
        {
            foreach (var var in cController.listCharSlot)
            {
                GameObject assetUI = Instantiate(characterBoxUI);
                charListUI.Add(assetUI);
                //assetUI.GetComponent<ArmouryCharacterBoxUI>().charName.text = var.characterBase.Name;
                assetUI.transform.SetParent(parentArmouryList.transform);
            }

            callOnce = false;
        }
    }
}
