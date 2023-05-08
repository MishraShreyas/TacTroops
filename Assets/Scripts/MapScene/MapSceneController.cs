using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneController : MonoBehaviour
{
    public static MapSceneController instance;
    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene(string type)
    {
        SceneManager.LoadScene(type);
    }

    public void NewMap()
    {
        int bs = PlayerPrefs.GetInt("Battles");
        if (bs >= 10) SceneManager.LoadScene("Shop");
        int m = PlayerPrefs.GetInt("Map");
        PlayerPrefs.SetInt("Map", m+1);
        PlayerPrefs.SetInt("ReloadMap", 1);
        SceneManager.LoadScene("Map");
    }
}
