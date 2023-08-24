using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmourySlot : MonoBehaviour
{
    private GameController cController;

    public UserCharactersSlot charSlot;
    public GameObject targetCharSlot;

    public Image charImage;
    public Text charName;
    public Text charLV;
    public Text starRating;
    public Text mainSkillDesc;
    public Text subSkillDesc;

    public void Start()
    {
        cController = FindObjectOfType<GameController>();
    }

    public void Update()
    {
        if (charSlot.characterBase != null)
        {
            if (charImage != null)
            {
                //charImage.sprite = charSlot.characterBase.IconSprite;
            }
            if (charName != null)
            {
                charName.text = charSlot.characterBase.Name;
            }
            if (charLV != null)
            {
                charLV.text = "LV" + charSlot.level.ToString();
            }
            if (starRating != null)
            {

            }
            if (mainSkillDesc != null)
            {

            }
            if (subSkillDesc != null)
            {

            }
        }
        else
        {
            if (charImage != null)
            {
                charImage.sprite = null;
            }
            if (charName != null)
            {
                charName.text = "";
            }
            if (charLV != null)
            {
                charLV.text = "";
            }
            if (starRating != null)
            {
                starRating.text = "";
            }
            if (mainSkillDesc != null)
            {

            }
            if (subSkillDesc != null)
            {

            }
        }
    }

    public void TransferDataArmoury()
    {
        if (targetCharSlot != null)
        {
            targetCharSlot.GetComponent<ArmourySlot>().charSlot = charSlot;

            cController.bUnitSlot.battleUnit = targetCharSlot.GetComponent<ArmourySlot>().charSlot;
        }
    }
}


