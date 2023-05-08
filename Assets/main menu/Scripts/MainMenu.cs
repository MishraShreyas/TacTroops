using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public CharacterStats[] stats;
    public GameObject continueButton;

    private void Start() {
        if (continueButton)
        if (!PlayerPrefs.HasKey("Lives") || PlayerPrefs.GetInt("Lives") <= 0)
        {
            continueButton.GetComponent<Button>().enabled = false;
            continueButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = Color.gray;
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene("Map");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("Battles", 0);
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("ReloadMap", 1);
        PlayerPrefs.SetInt("Map", 1);
        PlayerPrefs.SetInt("LivesTut", -1);
        PlayerPrefs.SetInt("CoinsTut", 0);
        PlayerPrefs.SetInt("MapTut", 0);
        PlayerPrefs.SetInt("TroopTut", 0);
        PlayerPrefs.SetInt("PowerupTut", 0);
        PlayerPrefs.SetInt("BattleTut", 0);
        PlayerStats.instance.stats = stats;
        PlayerStats.instance.SaveData();
        SceneManager.LoadScene("Map");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
