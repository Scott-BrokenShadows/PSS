using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStageList : MonoBehaviour
{
    [HideInInspector] public List<GameObject> stageList;
    //[ReadOnly] 
    [SerializeField] GameObject currentSelectedStage;
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
            //StartCoroutine(LoadYourAsyncScene());
            currentSelectedStage.transform.SetParent(null);
            DontDestroyOnLoad(currentSelectedStage);
            GoToStage();
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync($"{currentSelectedStage.GetComponent<LevelStage>().lStageScene}");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void GoToStage()
    {
        SceneManager.LoadScene($"{currentSelectedStage.GetComponent<LevelStage>().lStageScene}", LoadSceneMode.Single);
    }
}
