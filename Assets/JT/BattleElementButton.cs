using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleElementButton : MonoBehaviour
{
    public BattleUnit mainUnit;
    Button eButton;

    public Image outerBar;
    public Image innerBar;
    public Image fillBar;
    public Image iconSprite;
    public Text skillName;
    public Text stackSkill;

    [ReadOnly] public int currentStack;
    [ReadOnly] public float cAmount;
    [ReadOnly] public float cActiveTimer;
    [ReadOnly] public float cReloadTimer;
    [ReadOnly] public bool currentActive;

    public void Start()
    {
        // Set up the data
        eButton = GetComponent<Button>();
        eButton.interactable = (mainUnit != null) ? true : false;
        if (mainUnit != null) { SetUp(); }
    }

    public void Update()
    {
        if (mainUnit != null)
        {
            if (cAmount >= 1 && currentStack < mainUnit.HBCharacter.Base.UnitSkill.StackSkill)
            {
                currentStack++;
                currentStack = Mathf.Clamp(currentStack, 0, mainUnit.HBCharacter.Base.UnitSkill.StackSkill);
                stackSkill.text = $"{currentStack}/{mainUnit.HBCharacter.Base.UnitSkill.StackSkill}";
                
                cAmount = 0;
                cReloadTimer = mainUnit.HBCharacter.Base.UnitSkill.SkillReload;
            }

            if (currentStack != mainUnit.HBCharacter.Base.UnitSkill.StackSkill)
            {
                cReloadTimer -= Time.deltaTime;

                cAmount = BattleSystem.Remap(cReloadTimer, mainUnit.HBCharacter.Base.UnitSkill.SkillReload, 0f, 0f, 1f);
                fillBar.fillAmount = cAmount;
            }

            if (currentActive && currentStack > 0) { CurrentActive(); }
            eButton.interactable = (currentActive) ? false : true;

            return;
        }

        return;
    }

    void CurrentActive()
    {
        cActiveTimer -= Time.deltaTime;

        if (cActiveTimer <= 0)
        {
            currentActive = false;

            // reset timer
            cActiveTimer = mainUnit.HBCharacter.Base.UnitSkill.SkillActive;
        }
    }

    void SetUp()
    {
        cActiveTimer = mainUnit.HBCharacter.Base.UnitSkill.SkillActive;
        cReloadTimer = mainUnit.HBCharacter.Base.UnitSkill.SkillReload;

        cAmount = 0;
        fillBar.fillAmount = cAmount;

        skillName.text = mainUnit.HBCharacter.Base.UnitSkill.Name;
        stackSkill.text = $"{currentStack}/{mainUnit.HBCharacter.Base.UnitSkill.StackSkill}";
        iconSprite.sprite = mainUnit.HBCharacter.Base.UnitSkill.IconSprite;
    }

    // Activate when press button
    public void SkillButtonActivation()
    {
        if (!currentActive && currentStack > 0)
        {
            currentStack--;
            stackSkill.text = $"{currentStack}/{mainUnit.HBCharacter.Base.UnitSkill.StackSkill}";
            currentActive = true;
        }

        mainUnit.SkillActivation();
    }
}
