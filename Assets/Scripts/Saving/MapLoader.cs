using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    //CHANGE PATH TO PERSISTANT BEFORE BUILD
    public static MapLoader instance;
    private void Awake() {
        instance = this;
    }

    public MapGen mapGen;
    public MapData mapData;
    private string path = "";
    private string persistentPath = "";
    private string fileSuffix = "MapData.json";
    
    private void Start() {
        SetPaths();
        if (!PlayerPrefs.HasKey("Map"))
        {
            PlayerPrefs.SetInt("Map", 1);
            PlayerPrefs.SetInt("ReloadMap", 1);
        }
        if (PlayerPrefs.GetInt("ReloadMap") == 0) LoadData();
        else {
            mapGen.GenerateMap();
            mapData = new MapData(mapGen.adjList, mapGen.grid, -1, 0);
            SaveData();
        }
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

        string json = JsonUtility.ToJson(mapData);
        Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath + fileSuffix);
        writer.Write(json);
        
    }

    public void LoadData()
    {
        string savePath = persistentPath;

        using StreamReader reader = new StreamReader(savePath+fileSuffix);
        string json = reader.ReadToEnd();

        MapData data = JsonUtility.FromJson<MapData>(json);
        mapData = data;
        
        mapGen.adjList[0] = data.node1;
        mapGen.adjList[1] = data.node2;
        mapGen.adjList[2] = data.node3;
        mapGen.adjList[3] = data.node4;
        mapGen.adjList[4] = data.node5;
        mapGen.adjList[5] = data.node6;
        mapGen.adjList[6] = data.node7;
        mapGen.adjList[7] = data.node8;
        mapGen.adjList[8] = data.node9;
        mapGen.adjList[9] = data.node10;

        mapGen.currLayer = data.currLayer;
        mapGen.currNode = data.currNode;

        if (PlayerPrefs.GetInt("ActionComplete") != 1)
        {
            mapGen.currNode = data.prevNode;
            if (data.currLayer>0) mapGen.currLayer--;
        }
        mapGen.GenMapFromSave(data.layer1, data.layer2, data.layer3, data.layer4);
        
        Debug.Log("Loaded Data");
    }
}
