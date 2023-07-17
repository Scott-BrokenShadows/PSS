using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelWave
{
    [HideInInspector] public string name;

    public float countDownTimer;
    public List<HBEnemyList> EnemyList;
}

[System.Serializable]
public class HBEnemyList
{
    [HideInInspector] public string name;

    // Get the data from the database
    [SerializeField] public HBCharacterBase _base;
    public HBCharacter HBCharacter { get; set; }
    public int lv;
    [Min(1)] public int row;
    public float position;
    public float timer;
    public bool callOnce;
}

public class LevelStage : MonoBehaviour
{
    [LabelOverride("StageScene")]
    public string lStageScene;
    [Min(0)] public int gridRows = 5;

    public List<LevelWave> levelWave;

    private void OnValidate()
    {
        for (int i = 0; i < levelWave.Count; i++)
        {
            levelWave[i].name = $"Wave {(i + 1).ToString()}";

            for (int b = 0; b < levelWave[i].EnemyList.Count; b++)
            {
                if (levelWave[i].EnemyList[b]._base != true) { levelWave[i].EnemyList[b].name = $"Enemy Slot {(b + 1).ToString()}"; }
            }
        }
    }

    public List<LevelWave> LevelWave { get { return levelWave; } }
}
