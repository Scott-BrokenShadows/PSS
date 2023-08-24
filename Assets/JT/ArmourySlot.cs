using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmourySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UserCharactersSlot charSlot;
    public Image charImage;
    public Text charName;
    public Text charLV;
    public Text starRating;
    public Text mainSkillDesc;
    public Text subSkillDesc;
    
    int slotIndex;

    public void Update()
    {
        if (charSlot != null)
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
                charLV.text = charSlot.level.ToString();
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

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Move item from Armoury to Main
        Armoury.Instance.ArmouryToMain(slotIndex);

        //Move item from Armoury to Sub
        Armoury.Instance.ArmouryToSub(slotIndex);
    }

    //Set the Slot index
    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    //Display the item info on the item info box when the player mouses over
    public void OnPointerEnter(PointerEventData eventData)
    {
        Armoury.Instance.DisplayItemInfo(charSlot);
    }

    //Reset the item info box when the player leaves
    public void OnPointerExit(PointerEventData eventData)
    {
        Armoury.Instance.DisplayItemInfo(null);
    }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ArmourySlot : MonoBehaviour
//{
//    UserCharactersSlot charToDisplay;
//    public Image charDisplayImage;

//    public void Display(UserCharactersSlot charToDisplay)
//    {
//        // Check if there is an item to Display
//        if (charToDisplay != null)
//        {
//            charDisplayImage.sprite = charToDisplay.characterBase.IconSprite;
//            this.charToDisplay = charToDisplay;

//            charDisplayImage.gameObject.SetActive(true);

//            return;
//        }

//        charDisplayImage.gameObject.SetActive(false);
//    }
//}


