using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    // Inspector------------------------------------------------------------------------

    #region Inspector
    public GameObject battleUnit;
    public GameObject skillButtonUI;
    public GameObject hyperSkillButtonUI;
    public GameObject elementButtonUI;

    // Position
    [LabelOverride("Skill Button Position")]
    public Transform skillButtonPos;
    [LabelOverride("Hyper Skill Button Position")]
    public Transform hyperSkillButtonPos;
    [LabelOverride("Element Button Position")]
    public Transform elementButtonPos;

    [LabelOverride("Sub Unit Position")]
    public Transform subUnitPos;
    [LabelOverride("Main Unit Position")]
    public Transform mainUnitPos;

    // LevelStage
    [ReadOnly] private LevelStage currentLevel;

    // Whats going on with the current Wave
    [ReadOnly] private int currentWave;
    int cWave;
    [ReadOnly] private float cTimer;

    // The current Enemies on the game
    [HideInInspector] public List<LevelWave> currentWaveOnScene;
    [ReadOnly] public List<GameObject> cEnemy;

    // The current Players on the game
    [ReadOnly] private GameObject cPlayer;
    [ReadOnly] private GameObject cSubPlayer;

    // How many Grid Rows based on the LevelStage.cs
    #region Grid Rows
    [Min(0)] float range;
    int rows;

    [SerializeField] Vector2 screenSpace;
    public static Vector2 _screenSpace;
    [HideInInspector] public List<Vector3> GridRows;
    static public float vertical;
    static public float horizontal;
    [HideInInspector] public int cRows;
    #endregion

    [Range(0, 1)] [SerializeField] List<float> laneSlowDown;
    static public List<float> _laneSlowDown;

    // Complete Instantiateing Everything
    [HideInInspector] public bool cStage;

    [Separator]
    // Win Lose
    [SerializeField] GameObject battleOverHud;
    [SerializeField] GameObject winHud;
    [SerializeField] GameObject loseHud;

    [SerializeField] Text character1GainEXP;
    [SerializeField] Text character2GainEXP;
    [SerializeField] Text rewardMoney;

    bool allEnemiesNull = true;
    public static bool gBattleOver;
    bool winLose;

    [Separator]
    // Testing
    [SerializeField] BattleUnitSlot bUnitSlot;
    #endregion

    #region Inspector OnValidate 
    private void OnValidate()
    {
        // Loop through the list (starting from the second element)
        for (int i = 1; i < laneSlowDown.Count; i++)
        {
            // Clamp the current float to a maximum of 1
            laneSlowDown[i] = Mathf.Clamp(laneSlowDown[i], 0f, 1f);

            // Limit the current float by the previous float
            laneSlowDown[i] = Mathf.Min(laneSlowDown[i], laneSlowDown[i - 1]);
        }
    }
    #endregion

    // Start & Update------------------------------------------------------------------------

    #region Start & Update
    private void Start()
    {
        SetupBattle();
    }

    private void Update()
    {
        if (!gBattleOver)
        {
            if (!cStage)
            {
                TimerWave();
                InstanceEnemies();
            }

            allEnemiesNull = true;

            foreach (var enemy in cEnemy)
            {
                if (enemy != null)
                {
                    allEnemiesNull = false;
                }
            }

            if (cStage && allEnemiesNull)
            {
                winLose = true;
                BattleEnd();
            }

            if (cPlayer.GetComponent<BattleUnit>().HBCharacter.HP <= 0)
            {
                winLose = false;
                BattleEnd();
            }
        }
    }
    #endregion

    // Get Grid Rows ------------------------------------------------------------------------

    #region Get Grid Rows
    // Calculate the Grid Wave positions
    Ray[] GetGridRows(Transform origin, float range, int count)
    {
        Ray[] rays = new Ray[count];
        float s = range / (count - 1);
        float a = -range / 2f;
        for (int i = 0; i < count; i++)
        {
            float ca = a + i * s;
            rays[i].origin = new Vector3(0, ca, 0) / 1.5f + origin.position;
            rays[i].direction = -origin.right;
        }
        return rays;
    }
    #endregion

    // Start Up Mechanics------------------------------------------------------------------------

    #region Setup
    // Do what on the start
    void SetupBattle()
    {
        // BattleOverHud OFF
        BattleEndSetUp();

        // Lane to stop
        _laneSlowDown = laneSlowDown;

        // Add the extra screenspace
        _screenSpace = screenSpace;

        // Get the current Stage Information
        currentLevel = FindObjectOfType<LevelStage>();

        // Set the current Wave 
        currentWave = cWave + 1;

        // Set the current Stage Information
        GetEnemyWave();

        // Create how many Grid Rows
        #region Grid Rows
        rows = currentLevel.gridRows;
        cRows = rows;

        vertical = (float)Camera.main.orthographicSize;
        horizontal = vertical * (float)Camera.main.aspect;

        range = vertical * 2;

        foreach (var r in GetGridRows(transform, range, rows))
        {
            GridRows.Add(-r.origin);
        }
        #endregion

        if (currentWave > 0)
        // The current timer for each wave
        cTimer = currentWaveOnScene[cWave].countDownTimer;

        // Instantiate the Player Characters
        InstancePlayers();
        InstanceSubPlayers();

        mainUnitPos.GetComponent<BattlePlayerControl>().speed =
        ((bUnitSlot.battleUnit.characterBase.Speed / 999f) * 15f);
    }
    #endregion

    // Instantiate Mechanics------------------------------------------------------------------------

    #region Instance Enemy Unit
    // Timer Wave: when the timer reaches "0" go next wave
    void TimerWave()
    {
        if (currentWaveOnScene.Count - 1 > cWave)
        {
            cTimer -= Time.deltaTime;

            if (cTimer <= 0)
            {
                cWave++;
                GetEnemyWave();
                cTimer = currentWaveOnScene[cWave].countDownTimer;
            }
        }
        else if (currentWaveOnScene.Count - 1 == cWave)
        {
            cTimer -= Time.deltaTime;

            if (cTimer <= 0)
            {
                cTimer = 0;
                cStage = true;
                Debug.Log("Current Stage = " + cStage);
            }
        }
    }

    // Get the information on the current Wave
    void GetEnemyWave()
    {
        currentWaveOnScene = currentLevel.levelWave;
    }

    // Instantiate Enemies from the information based on the timer to spawn
    void InstanceEnemies()
    {
        for (int b = 0; b < currentWaveOnScene[cWave].EnemyList.Count; b++)
        {
            if (currentWaveOnScene[cWave].EnemyList[b].callOnce != true)
            {
                //Timer goes down to spawn
                currentWaveOnScene[cWave].EnemyList[b].timer -= Time.deltaTime;

                if (currentWaveOnScene[cWave].EnemyList[b].timer <= 0)
                {
                    currentWaveOnScene[cWave].EnemyList[b].timer = 0;

                    // Instantiate the battleUnit with the data from the LevelStage
                    GameObject bUnit = Instantiate(battleUnit, new Vector3(currentWaveOnScene[cWave].EnemyList[b].position, GridRows[currentWaveOnScene[cWave].EnemyList[b].row - 1].y, 0), Quaternion.identity);
                    bUnit.GetComponent<BattleUnit>()._base = currentWaveOnScene[cWave].EnemyList[b]._base;
                    bUnit.GetComponent<BattleUnit>().level = currentWaveOnScene[cWave].EnemyList[b].lv;
                    bUnit.GetComponent<BattleUnit>().sLane = currentWaveOnScene[cWave].EnemyList[b].stopLane;
                    bUnit.GetComponent<BattleUnit>().isPlayer = false;
                    cEnemy.Add(bUnit);
                    currentWaveOnScene[cWave].EnemyList[b].callOnce = true;
                }
            }
        }
    }
    #endregion

    #region Instance Player Unit
    void InstancePlayers()
    {
        GameObject asset = Instantiate(battleUnit, transform);
        //asset.transform.position = frontUnitPos.position;
        cPlayer = asset;
        asset.GetComponent<BattleUnit>()._base = bUnitSlot.battleUnit.characterBase;
        asset.GetComponent<BattleUnit>().level = bUnitSlot.battleUnit.level;
        asset.GetComponent<BattleUnit>().isPlayer = true;
        asset.GetComponent<BattleUnit>().subUnit = false;
        asset.transform.SetParent(null);

        // Element
        GameObject assetUI = Instantiate(elementButtonUI, transform);
        assetUI.transform.SetParent(elementButtonPos.transform);
        assetUI.transform.position = elementButtonPos.position;
        assetUI.transform.localScale = new Vector3(1f, 1f, 1f);

        // HyperSkill
        GameObject assetHS = Instantiate(hyperSkillButtonUI, transform);
        assetHS.transform.SetParent(hyperSkillButtonPos.transform);
        assetHS.transform.position = hyperSkillButtonPos.position;
        assetHS.transform.localScale = new Vector3(1f, 1f, 1f);
        assetHS.GetComponent<BattleHyperSkillButton>().mainUnit = asset.GetComponent<BattleUnit>();
        assetHS.GetComponent<BattleHyperSkillButton>().bSystem = this;
    }
    void InstanceSubPlayers()
    {
        GameObject asset = Instantiate(battleUnit, transform);
        cSubPlayer = asset;
        asset.transform.position = subUnitPos.position;
        asset.GetComponent<BattleUnit>()._base = bUnitSlot.subBattleUnit.characterBase;
        asset.GetComponent<BattleUnit>().level = bUnitSlot.subBattleUnit.level;
        asset.GetComponent<BattleUnit>().isPlayer = true;
        asset.GetComponent<BattleUnit>().subUnit = true;
        asset.transform.SetParent(null);

        // Skill
        GameObject assetUI = Instantiate(skillButtonUI, transform);
        assetUI.transform.SetParent(skillButtonPos.transform);
        assetUI.transform.position = skillButtonPos.position;
        assetUI.transform.localScale = new Vector3(1f, 1f, 1f);
        assetUI.GetComponent<BattleSkillButton>().subUnit = asset.GetComponent<BattleUnit>();
    }
    #endregion

    // Remap Function------------------------------------------------------------------------

    #region Remap Function
    static public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
    #endregion

    // EXP Function------------------------------------------------------------------------

    #region EXP Function
    public void GainXP(ref int currentLevel, ref int currentXP, int xpIncreasePerLevel, int amount)
    {
        currentXP += amount;

        int xpRequiredForNextLevel = currentLevel * xpIncreasePerLevel;

        if (currentXP >= xpRequiredForNextLevel)
        {
            int leftoverXP = currentXP - xpRequiredForNextLevel;
            currentLevel++;
            currentXP = leftoverXP;

            Debug.Log("Level Up! Current level: " + currentLevel);
        }
    }
    #endregion

    // GameOver Function------------------------------------------------------------------------

    #region GameOver Function

    void BattleEndSetUp()
    {
        battleOverHud.SetActive(false);
        winHud.SetActive(false);
        loseHud.SetActive(false);
    }

    public void BattleEnd()
    {
        BattleGameEnd();
        gBattleOver = true;
        Debug.Log("Game Battle End = " + gBattleOver);
    }

    public void BattleGameEnd()
    {
        skillButtonPos.gameObject.SetActive(false);
        hyperSkillButtonPos.gameObject.SetActive(false);
        elementButtonPos.gameObject.SetActive(false);

        Destroy(subUnitPos.gameObject);
        Destroy(mainUnitPos.gameObject);

        battleOverHud.SetActive(true);
        if (winLose) { BattleGameWin(); } else { BattleGameLose(); };
    }

    public void BattleGameWin()
    {
        Debug.Log("Win");
        winHud.SetActive(true);

        // Main Character 100%
        //bUnitSlot.battleUnit.currentXP += currentLevel.rewards.rEXP;
        GainXP(ref bUnitSlot.battleUnit.level, ref bUnitSlot.battleUnit.currentXP, 1000, currentLevel.rewards.rEXP);
        character1GainEXP.text = $"+{currentLevel.rewards.rEXP} EXP";

        // Sub character 50%
        //bUnitSlot.subBattleUnit.currentXP += currentLevel.rewards.rEXP / 2;
        GainXP(ref bUnitSlot.subBattleUnit.level, ref bUnitSlot.subBattleUnit.currentXP, 1000, currentLevel.rewards.rEXP / 2);
        character2GainEXP.text = $"+{currentLevel.rewards.rEXP / 2} EXP";
    }

    public void BattleGameLose()
    {
        Debug.Log("Lose");
        loseHud.SetActive(true);

        character1GainEXP.text = $"+0 EXP";
        character2GainEXP.text = $"+0 EXP";
        rewardMoney.text = $"+$0";
    }
    #endregion

    // Show Gizmos------------------------------------------------------------------------

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        #region Grid Rows
        if (currentLevel)
        {
            foreach (var r in GetGridRows(transform, range, cRows))
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(new Vector3(horizontal, r.origin.y), new Vector3(-horizontal, r.origin.y));
            }
        }
        #endregion
        #region Borders
        Gizmos.DrawLine(new Vector3(-horizontal, -vertical), new Vector3(horizontal, -vertical));
        Gizmos.DrawLine(new Vector3(-horizontal, vertical), new Vector3(horizontal, vertical));
        Gizmos.DrawLine(new Vector3(-horizontal, -vertical), new Vector3(-horizontal, vertical));
        Gizmos.DrawLine(new Vector3(horizontal, -vertical), new Vector3(horizontal, vertical));
        #endregion

        Gizmos.color = Color.yellow;
        #region Outside Borders
        Gizmos.DrawLine(new Vector3(-horizontal - _screenSpace.x, -vertical - _screenSpace.y),
                        new Vector3(horizontal + _screenSpace.x, -vertical - _screenSpace.y));
        Gizmos.DrawLine(new Vector3(-horizontal - _screenSpace.x, vertical + _screenSpace.y),
                        new Vector3(horizontal + _screenSpace.x, vertical + _screenSpace.y));
        Gizmos.DrawLine(new Vector3(-horizontal - _screenSpace.x, -vertical - _screenSpace.y),
                        new Vector3(-horizontal - _screenSpace.x, vertical + _screenSpace.y));
        Gizmos.DrawLine(new Vector3(horizontal + _screenSpace.x, -vertical - _screenSpace.y),
                        new Vector3(horizontal + _screenSpace.x, vertical + _screenSpace.y));
        #endregion

        Gizmos.color = Color.red;
        #region Enemy Lane Stop
        if (laneSlowDown != null)
        {
            for (int i = 0; i < laneSlowDown.Count; i++)
            {
                Gizmos.DrawLine(new Vector3(Remap(laneSlowDown[i], 0, 1, -horizontal, horizontal), -vertical),
                    new Vector3(Remap(laneSlowDown[i], 0, 1, -horizontal, horizontal), vertical));
            }
        }
        #endregion
    }
    #endregion
}
