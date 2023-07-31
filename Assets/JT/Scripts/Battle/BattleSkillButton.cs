using Fungus;
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

    [ReadOnly] public int currentStack;
    [ReadOnly] public float cAmount;
    [ReadOnly] public float cActiveTimer;
    [ReadOnly] public float cReloadTimer;
    [ReadOnly] public bool currentActive;

    public void Start()
    {
        // Set up the data
        sButton = GetComponent<Button>();
        sButton.interactable = (subUnit != null) ? true : false;
        if (subUnit != null) { SetUp(); }
    }

    public void Update()
    {
        if (subUnit != null)
        {
            if (cAmount >= 1 && currentStack < subUnit.HBCharacter.Base.UnitSkill.StackSkill)
            {
                currentStack++;
                currentStack = Mathf.Clamp(currentStack, 0, subUnit.HBCharacter.Base.UnitSkill.StackSkill);
                stackSkill.text = $"{currentStack}/{subUnit.HBCharacter.Base.UnitSkill.StackSkill}";
                
                cAmount = 0;
                cReloadTimer = subUnit.HBCharacter.Base.UnitSkill.SkillReload;
            }

            if (currentStack != subUnit.HBCharacter.Base.UnitSkill.StackSkill)
            {
                cReloadTimer -= Time.deltaTime;

                cAmount = BattleSystem.Remap(cReloadTimer, subUnit.HBCharacter.Base.UnitSkill.SkillReload, 0f, 0f, 1f);
                fillBar.fillAmount = cAmount;
            }

            if (currentActive && currentStack > 0) { CurrentActive(); }
            sButton.interactable = (currentActive) ? false : true;

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
            cActiveTimer = subUnit.HBCharacter.Base.UnitSkill.SkillActive;
        }
    }

    void SetUp()
    {
        cActiveTimer = subUnit.HBCharacter.Base.UnitSkill.SkillActive;
        cReloadTimer = subUnit.HBCharacter.Base.UnitSkill.SkillReload;

        cAmount = 0;
        fillBar.fillAmount = cAmount;

        skillName.text = subUnit.HBCharacter.Base.UnitSkill.Name;
        stackSkill.text = $"{currentStack}/{subUnit.HBCharacter.Base.UnitSkill.StackSkill}";
        iconSprite.sprite = subUnit.HBCharacter.Base.UnitSkill.IconSprite;
    }

    // Activate when press button
    public void SkillButtonActivation()
    {
        if (!currentActive && currentStack > 0)
        {
            currentStack--;
            stackSkill.text = $"{currentStack}/{subUnit.HBCharacter.Base.UnitSkill.StackSkill}";
            currentActive = true;
        }

        subUnit.SkillActivation();
    }
}
