using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStageSelect : MonoBehaviour
{
    [HideInInspector] public GameController cController;

    public void Start()
    {
        cController = FindObjectOfType<GameController>();
    }
    public void LevelSelect()
    {
        GameObject asset = Instantiate(this.gameObject, transform);
        cController._cStage = asset;
        this.transform.SetParent(null);
        DontDestroyOnLoad(this);
        //GoToStage();
        GoToStageSingle();
    }

    public void LevelSelectSingle()
    {
        //this.transform.SetParent(null);
        //DontDestroyOnLoad(this);
        cController._cStage = this.gameObject;
        GoToStageSingle();
    }

    void GoToStage()
    {
        SceneManager.LoadScene($"{this.GetComponent<LevelStage>().lStageScene}", LoadSceneMode.Additive);
    }

    void GoToStageSingle()
    {
        SceneManager.LoadScene($"{this.GetComponent<LevelStage>().lStageScene}", LoadSceneMode.Single);
    }
}
