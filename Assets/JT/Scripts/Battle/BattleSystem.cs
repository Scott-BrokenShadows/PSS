using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public GameObject battleUnit;

    // LevelStage
    [ReadOnly] public LevelStage currentLevel;

    // Whats going on with the current Wave
    [ReadOnly] public int currentWave;
    int cWave;
    [ReadOnly] public float cTimer;

    // The current Enemies on the game
    public List<LevelWave> currentWaveOnScene;
    [ReadOnly] public List<GameObject> cEnemy;

    // How many Grid Rows based on the LevelStage.cs
    #region Grid Rows
    [Min(0)] float range;
    int rows;

    [SerializeField] Vector2 screenSpace;
    public static Vector2 _screenSpace;
    [HideInInspector] public List<Vector3> GridRows;
    static public float vertical;
    static public float horizontal;
    public int cRows;
    #endregion

    private void Start()
    {
        SetupBattle();
    }

    private void Update()
    {
        TimerWave();
        InstanceEnemies();
    }

    // Calculate the Grid Wave positions
    #region Get Grid Rows
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

    // Do what on the start
    void SetupBattle()
    {
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
                    bUnit.GetComponent<HBCharacterBattleUnits>()._base = currentWaveOnScene[cWave].EnemyList[b]._base;
                    bUnit.GetComponent<HBCharacterBattleUnits>().isPlayer = false;
                    cEnemy.Add(bUnit);

                    currentWaveOnScene[cWave].EnemyList[b].callOnce = true;
                }
            }
        }
    }

    void InstancePlayers()
    { 
    
    }
    void InstanceSubPlayers()
    {

    }

    // Just show gizmos
    private void OnDrawGizmos()
    {
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(-horizontal - _screenSpace.x, -vertical - _screenSpace.y), 0.25f);
        Gizmos.DrawSphere(new Vector3(horizontal + _screenSpace.x, -vertical - _screenSpace.y), 0.25f);
        Gizmos.DrawSphere(new Vector3(-horizontal - _screenSpace.x, vertical + _screenSpace.y), 0.25f);
        Gizmos.DrawSphere(new Vector3(horizontal + _screenSpace.x, vertical + _screenSpace.y), 0.25f);

        Gizmos.DrawLine(new Vector3(-horizontal - _screenSpace.x, -vertical - _screenSpace.y),
                        new Vector3(horizontal + _screenSpace.x, -vertical - _screenSpace.y));
        Gizmos.DrawLine(new Vector3(-horizontal - _screenSpace.x, vertical + _screenSpace.y),
                        new Vector3(horizontal + _screenSpace.x, vertical + _screenSpace.y));

        Gizmos.DrawLine(new Vector3(-horizontal - _screenSpace.x, -vertical - _screenSpace.y),
                        new Vector3(-horizontal - _screenSpace.x, vertical + _screenSpace.y));
        Gizmos.DrawLine(new Vector3(horizontal + _screenSpace.x, -vertical - _screenSpace.y),
                        new Vector3(horizontal + _screenSpace.x, vertical + _screenSpace.y));
    }
}
