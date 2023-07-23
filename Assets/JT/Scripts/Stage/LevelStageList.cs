using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStageList : MonoBehaviour
{
    [HideInInspector] public List<GameObject> stageList;
    [ReadOnly] [SerializeField] GameObject currentSelectedStage;
    int currentInt;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            stageList.Add(child.gameObject);
        }
    }

    void Update()
    {
        currentInt = Mathf.Clamp(currentInt, 0, (stageList.Count - 1));
        currentSelectedStage = stageList[currentInt];

        if (Input.GetKeyDown(KeyCode.W))
        {
            print("W key was pressed");
            currentInt--;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            print("S key was pressed");
            currentInt++;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentSelectedStage.transform.SetParent(null);
            DontDestroyOnLoad(currentSelectedStage);
            GoToStage();
        }
    }

    void GoToStage()
    {
        SceneManager.LoadScene($"{currentSelectedStage.GetComponent<LevelStage>().lStageScene}", LoadSceneMode.Single);
    }
}
