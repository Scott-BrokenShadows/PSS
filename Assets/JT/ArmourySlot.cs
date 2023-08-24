using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmourySlot : MonoBehaviour
{
    UserCharactersSlot charToDisplay;
    public Image charDisplayImage;

    public void Display(UserCharactersSlot charToDisplay)
    {
        // Check if there is an item to Display
        if (charToDisplay != null)
        {
            charDisplayImage.sprite = charToDisplay.characterBase.IconSprite;
            this.charToDisplay = charToDisplay;

            charDisplayImage.gameObject.SetActive(true);

            return;
        }

        charDisplayImage.gameObject.SetActive(false);
    }
}
