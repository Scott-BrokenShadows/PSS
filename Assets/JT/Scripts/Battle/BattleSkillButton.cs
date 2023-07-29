using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillButton : MonoBehaviour
{
    public BattleUnit subUnit;
    Button sButton;

    public Image outerBar;
    public Image innerBar;
    public Image fillBar;
    public Image iconSprite;
    public Text skillName;
    public Text stackSkill;

    float cAmount;

    public void Start()
    {
        sButton = GetComponent<Button>();
        sButton.interactable = (subUnit != null) ? true : false;

        cAmount = 0;
        fillBar.fillAmount = cAmount;
    }

    public void Update()
    {
        if (subUnit != null)
        {
            cAmount += Time.deltaTime;
            fillBar.fillAmount = cAmount;
            return;
        }
    }

    // Activate when press button
    public void SkillButtonActivation()
    {
        subUnit.SkillActivation();
    }
}
