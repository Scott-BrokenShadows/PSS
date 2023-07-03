using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public GameObject battleUnit;

    [ReadOnly] public LevelStage currentLevel;
    [ReadOnly] public int currentWave;
    int cWave;
    [ReadOnly] public float cTimer;
    public List<GameObject> currentEnemyOnScene;
    public List<float> currentEnemyTimer;
    public List<bool> currentEnemyCallOnce;

    #region Grid Rows
    [Min(0)] float range;
    int rows;

    [HideInInspector] public List<Vector3> GridRows;
    float vertical;
    public int cRows;
    #endregion

    private void Start()
    {
        SetupBattle();
    }

    private void Update()
    {
        currentWave = cWave + 1;
        TimerWave();
        InstanceEnemies();
    }

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

    void SetupBattle()
    {
        currentLevel = FindObjectOfType<LevelStage>();

        for (int b = 0; b < currentLevel.LevelWave[cWave].EnemyList.Count; b++)
        {
            //currentLevel.LevelWave[cWave].EnemyList[b].cTimer = currentLevel.LevelWave[cWave].EnemyList[b].timer;
            currentEnemyTimer.Add(currentLevel.LevelWave[cWave].EnemyList[b].timer);
            currentEnemyCallOnce.Add(false);
        }

        #region Grid Rows
        rows = currentLevel.gridRows;
        cRows = rows;

        vertical = (float)Camera.main.orthographicSize;
        range = vertical * 2;

        foreach (var r in GetGridRows(transform, range, rows))
        {
            GridRows.Add(-r.origin);
        }
        #endregion

        cTimer = currentLevel.LevelWave[cWave].countDownTimer;
    }

    void TimerWave()
    {
        if (currentLevel.LevelWave.Count - 1 > cWave)
        {
            cTimer -= Time.deltaTime;

            if (cTimer <= 0)
            {
                cWave++;
                cTimer = currentLevel.LevelWave[cWave].countDownTimer;
            }
        }
        else if (currentLevel.LevelWave.Count - 1 == cWave)
        {
            cTimer -= Time.deltaTime;

            if (cTimer <= 0)
            {
                Debug.Log("Complete Stage");

                cTimer = 0;
            }
        }
    }

    void InstanceEnemies()
    {
        for (int b = 0; b < currentLevel.LevelWave[cWave].EnemyList.Count; b++)
        {
            //if (currentLevel.LevelWave[cWave].EnemyList[b].callOnce != true)
            if (currentEnemyCallOnce[b] != true)
            {
                //currentLevel.LevelWave[cWave].EnemyList[b].cTimer -= Time.deltaTime;
                currentEnemyTimer[b] -= Time.deltaTime;

                //if (currentLevel.LevelWave[cWave].EnemyList[b].cTimer <= 0)
                if (currentEnemyTimer[b] <= 0)
                {
                    GameObject bUnit = Instantiate(battleUnit, new Vector3(currentLevel.LevelWave[cWave].EnemyList[b].position, GridRows[currentLevel.LevelWave[cWave].EnemyList[b].row - 1].y, 0), Quaternion.identity);
                    //Debug.Log(GridRows[currentLevel.LevelWave[cWave].EnemyList[b].row - 1].y);
                    bUnit.GetComponent<HBCharacterBattleUnits>()._base = currentLevel.LevelWave[cWave].EnemyList[b]._base;
                    bUnit.GetComponent<HBCharacterBattleUnits>().isPlayer = false;
                    currentEnemyOnScene.Add(bUnit);

                    //currentLevel.LevelWave[cWave].EnemyList[b].callOnce = true;
                    currentEnemyCallOnce[b] = true;

                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        #region Grid Rows
        if (currentLevel)
        {
            float horizontal = vertical * (float)Camera.main.aspect;

            foreach (var r in GetGridRows(transform, range, cRows))
            {
                //Debug.DrawRay(r.origin, r.direction * 5);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(new Vector3(horizontal, r.origin.y), new Vector3(-horizontal, r.origin.y));
            }
        }
        #endregion
    }
}
