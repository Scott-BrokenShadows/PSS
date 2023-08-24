using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmouryHoldSlot : ArmourySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        //Move item from Main to Armoury
        Armoury.Instance.MainToArmoury();

        //Move item from Sub to Armoury
        Armoury.Instance.SubToArmoury();
    }
}
