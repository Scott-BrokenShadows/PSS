using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHyperSkillButton : MonoBehaviour
{
    public BattleUnit mainUnit;
    public BattleSystem bSystem;
    Button hsButton;

    public Image outerBar;
    public Image innerBar;
    public Image fillBar;
    public Image iconSprite;
    public Text skillName;
    public Text stackSkill;

    [ReadOnly] public float cAmount;
    [ReadOnly] public float cReloadTimer;

    [Separator]
    public string hyperSkillName;
    public float skillReload;

    public void Start()
    {
        // Set up the data
        hsButton = GetComponent<Button>();
        hsButton.interactable = (mainUnit != null) ? true : false;
        if (mainUnit != null) { SetUp(); }
    }

    public void Update()
    {
        if (mainUnit != null)
        {
            if (cAmount >= 1)
            {
                hsButton.interactable = true;
            }

            if (cAmount < 1)
            {
                hsButton.interactable = false;
                cReloadTimer -= Time.deltaTime;

                cAmount = BattleSystem.Remap(cReloadTimer, skillReload, 0f, 0f, 1f);
                fillBar.fillAmount = cAmount;
            }
            return;
        }

        return;
    }

    void SetUp()
    {
        skillName.text = hyperSkillName;
        cReloadTimer = skillReload;

        cAmount = 0;
        fillBar.fillAmount = cAmount;
    }

    // Activate when press button
    public void SkillButtonActivation()
    {
        if (cAmount >= 1)
        {
            DestroyEnemiesInsideBorders();
            cAmount = 0;
            cReloadTimer = skillReload;
        }
    }

    void DestroyEnemiesInsideBorders()
    {
        if (transform.position.x > -BattleSystem.horizontal ||
            transform.position.x <  BattleSystem.horizontal ||
            transform.position.y > -BattleSystem.vertical   ||
            transform.position.y <  BattleSystem.vertical   )
        {
            foreach (var var in bSystem.cEnemy)
            //foreach (var var in BattleSystem.cEnemy)
            {
                Destroy(var);
            }
        }
    }
}
