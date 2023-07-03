using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    //public static ItemBaseA[] items;//static list of items that the inventory can reference for images and names

    public SaveData loadedData;//where data can be loaded into

    public SaveData testData;//just here for testing so you have something to save

    [Header("Buttons")]
    public bool save;//save
    public bool load;//load

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Application.dataPath);//shows where to find the save file
    }

    // Update is called once per frame
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
        //-- testData.pics = PhotoBook.main.allPictures;

        var json = JsonUtility.ToJson(testData); // convert savedata class to json string
        var path = Application.dataPath;//get file path for app
        var fullPath = Path.Combine(path, "saveGame.json");// add the save file name

        var p = path + "/pics";

        if (!Directory.Exists(p))
        {
            Directory.CreateDirectory(p);
            Debug.Log("pics folder not found, creating");
        }

        #region
        /*
        foreach (var item in testData.pics)
        {
            var fp = Path.Combine(p, item.UUID + ".jpg");

            if (!File.Exists(fp))//check if it doesnt exist yet
            {
                File.Create(fp).Close();//create it if it doesnt exist
            }
            File.WriteAllBytes(fp, item.texture.EncodeToJPG());
        }
        */
        #endregion

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        if (!File.Exists(fullPath))//check if it doesnt exist yet
        {
            File.Create(fullPath).Close();//create it if it doesnt exist
        }

        File.WriteAllText(fullPath, json);// write all json string to the file
    }
    void Load()
    {
        var path = Application.dataPath;// get path
        var fullPath = Path.Combine(path, "saveGame.json");//add file name
        if (File.Exists(fullPath))//if it exists
        {
            var s = File.ReadAllText(fullPath);//read file and get json serialized string
            loadedData = JsonUtility.FromJson<SaveData>(s);//create save data from raw json string
        }
        else //if file not found
        {
            Debug.Log("Data not found");//say we cant find it
        }
        path = path + "/pics";

        #region
        /*
        foreach (var item in loadedData.pics)
        {
            var p = Path.Combine(path, item.UUID + ".jpg");
            if (File.Exists(p))//if it exists
            {
                var b = File.ReadAllBytes(p);//read file and get byte array
                var t = new Texture2D(2, 2);
                t.LoadImage(b);
                item.texture = t;
            }
            else //if file not found
            {
                Debug.Log("picture not found");//say we cant find it
            }
        }
        */
        #endregion

        //-- PhotoBook.main.allPictures = loadedData.pics;

    }

}
[System.Serializable]
public class SaveData// this class gets saved to the file, add any data you want saved to this
{
    //-- public List<Picture> pics;
}
