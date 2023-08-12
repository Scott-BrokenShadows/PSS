using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Win Lose
    public static bool gBattleOver; 

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
            TimerWave();
            InstanceEnemies();
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
                // What to do when you complete the stage
                Debug.Log("Complete Stage");
                BattleGameEnd();
                cTimer = 0;
                gBattleOver = true;
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
    //public int currentLevel = 1;
    //public int currentXP = 0;
    //public int xpIncreasePerLevel = 100;

    //public void GainXP(int amount)
    //{
    //    currentXP += amount;
    //    CheckForLevelUp();
    //}

    //private void CheckForLevelUp()
    //{
    //    int xpRequiredForNextLevel = currentLevel * xpIncreasePerLevel;

    //    if (currentXP >= xpRequiredForNextLevel)
    //    {
    //        currentLevel++;
    //        Debug.Log("Level Up! Current level: " + currentLevel);
    //    }
    //}
    #endregion

    // GameOver Function------------------------------------------------------------------------

    #region GameOver Function
    public void BattleGameEnd()
    {
        skillButtonPos.gameObject.SetActive(false);
        hyperSkillButtonPos.gameObject.SetActive(false);
        elementButtonPos.gameObject.SetActive(false);

        foreach (var enemy in cEnemy)
        {
            enemy.GetComponent<BattleUnit>().enabled = false;
        }

        cPlayer.GetComponent<BattleUnit>().enabled = false;
        cPlayer.GetComponent<BattleUnit>().enabled = false;

        BattleGameWin();
        BattleGameLose();
    }

    public void BattleGameWin()
    { 
    
    }

    public void BattleGameLose()
    {

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
