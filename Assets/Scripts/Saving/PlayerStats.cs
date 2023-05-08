using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //CHANGE PATH TO PERSISTANT BEFORE BUILD
    public static PlayerStats instance;
    private void Awake() {
        instance = this;
    }

    public bool loaded=false;

    public CharacterStats[] stats;
    private string path = "";
    private string persistentPath = "";
    private string fileSuffix = "PlayerData.json";
    
    private void Start() {
        SetPaths();
        //SaveData(); //remove
        //PlayerPrefs.SetInt("ReloadMap", 1); //remove
        //PlayerPrefs.SetInt("Map", 1); //remove
        if (PlayerPrefs.GetInt("FirstTime")==1)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            SaveData();
        } else LoadData();

        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar;
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    public void SaveData()
    {
        string savePath = persistentPath;

        Debug.Log("Saving Data at " + savePath);
        foreach (CharacterStats chs in stats)
        {
            string json = JsonUtility.ToJson(chs);
            Debug.Log(json);

            using StreamWriter writer = new StreamWriter(savePath + chs.type + fileSuffix);
            writer.Write(json);
        }
        
    }

    public void LoadData()
    {
        loaded=true;
        string savePath = persistentPath;

        for (int i=0; i<stats.Length; i++)
        {
            CharacterStats chs = stats[i];
            using StreamReader reader = new StreamReader(savePath+chs.type+fileSuffix);
            string json = reader.ReadToEnd();

            CharacterStats data = JsonUtility.FromJson<CharacterStats>(json);
            stats[i] = data;
        }
        Debug.Log("Loaded Data");
    }
}
