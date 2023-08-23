using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armoury : MonoBehaviour
{
    [ReadOnly] public GameController cController;
    public Transform parentArmouryList;

    public GameObject characterBoxUI;
    [ReadOnly] public List<GameObject> charListUI;

    public bool callOnce;

    // Start is called before the first frame update
    void Start()
    {
        cController = FindObjectOfType<GameController>();
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
                assetUI.GetComponent<ArmouryCharacterBoxUI>().charName.text = var.characterBase.Name;
                assetUI.transform.SetParent(parentArmouryList.transform);
            }

            callOnce = false;
        }
    }
}
