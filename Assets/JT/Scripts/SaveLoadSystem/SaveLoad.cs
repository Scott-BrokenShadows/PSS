using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad SaveSystem { get; private set; }
    [HideInInspector] public SaveData loadedData;//where data can be loaded into
    [HideInInspector] public SaveData savedData;//just here for testing so you have something to save

    [Header("Buttons")]
    public bool save;//save
    public bool load;//load

    void Awake()
    {
        SaveSystem = this;
    }

    void Start()
    {
        //Debug.Log(Application.dataPath);//shows where to find the save file
    }

    void Update()
    {
        if (save)//basic bool button, fires once after it is clicked in the inspector
        {
            save = false;
            Save();
        }

        if (load)
        {
            load = false;
            Load();
        }
    }

    void Save()
    {
        var json = JsonUtility.ToJson(savedData); // convert savedata class to json string
        var path = Application.dataPath;//get file path for app
        var fullPath = Path.Combine(path, "SaveGame");// add the save file name
        if (!File.Exists(fullPath))//check if it doesnt exist yet
        {
            File.Create(fullPath).Close();//create it if it doesnt exist
        }

        File.WriteAllText(fullPath, json);// write all json string to the file
    }

    void Load()
    {
        var path = Application.dataPath;// get path
        var fullPath = Path.Combine(path, "SaveGame");//add file name
        if (File.Exists(fullPath))//if it exists
        {
            var s = File.ReadAllText(fullPath);//read file and get json serialized string
            loadedData = JsonUtility.FromJson<SaveData>(s);//create save data from raw json string
        }
        else //if file not found
        {
            Debug.Log("Data not found");//say we cant find it
        }
    }
}
[System.Serializable]
public class SaveData// this class gets saved to the file, add any data you want saved to this
{
    public string dateTime;
    public string gameplayTime;
    public int gameCurrency;
    public List<UserCharactersSlot> listCharacters;
    public List<UserCharactersSlot> listPartyBattleUnits;
    public List<ItemSlot> listItems;
}