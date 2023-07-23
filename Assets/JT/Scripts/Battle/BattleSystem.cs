using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    // Inspector------------------------------------------------------------------------

    #region Inspector
    public GameObject battleUnit;

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
    #endregion

    // Start & Update------------------------------------------------------------------------

    #region Start & Update
    private void Start()
    {
        SetupBattle();
    }

    private void Update()
    {
        TimerWave();
        InstanceEnemies();
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
            rays[i].origin = new Vector3(0, ca, 0) / 1.25f + origin.position;
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
                cTimer = 0;
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
                    bUnit.GetComponent<BattleUnit>().isPlayer = false;

                    //// Set the Unit Data
                    //bUnit.GetComponent<BattleUnit>().bUnitHud.SetData(bUnit.GetComponent<BattleUnit>().HBCharacter);

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
        
    }
    void InstanceSubPlayers()
    {

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
        if (_laneSlowDown != null)
        {
            for (int i = 0; i < _laneSlowDown.Count; i++)
            {
                Gizmos.DrawLine(new Vector3(Remap(_laneSlowDown[i], 0, 1, -horizontal, horizontal), -vertical),
                    new Vector3(Remap(_laneSlowDown[i], 0, 1, -horizontal, horizontal), vertical));
            }
        }
        #endregion
    }
    #endregion
}
