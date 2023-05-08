using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject cam1;
    public GameObject cam2;

    public Text remainingTroops;
    public Text remainingEnemies;

    public GameObject UISelect;
    public GameObject abilities;
    public GameObject controls;
    public GameObject finishDialogue;
    public Image[] TroopSelect;

    public Color normalClr;
    public Color SelectedClr;

    public bool menuOpen = false;
    public int canons=0;

    private GameManager _gm;
    bool dialogues = false;
    bool ended = false;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        remainingEnemies.text = "Enemies: "+_gm.enemies.ToString();
        remainingTroops.text = "Troops: "+_gm.troopers.ToString();


        if (Input.GetKeyDown(KeyCode.Return) && ended)
        {
            ended =false;
            if (PlayerPrefs.GetInt("Lives")<0) SceneManager.LoadScene("Defeat");
            else if (!_gm.boss) SceneManager.LoadScene("Map");
            else SceneManager.LoadScene("Credits");
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return) && !_gm.strt)
        {
            if (menuOpen) toggleMenu();
            CardController.instance.gameObject.SetActive(false);
            _gm.begin();
        }

        if (!dialogues && FindObjectsOfType<DialogueBox>().Length==0)
        {
            dialogues = true;
            controls.SetActive(true);
        }
    }

    public void toggleMenu()
    {
        menuOpen = !menuOpen;
        if (!_gm.strt) UISelect.SetActive(menuOpen);
        else abilities.SetActive(menuOpen);
        FreeFlyCamera.instance.enabled = !menuOpen;
        Cursor.visible = menuOpen;
        Cursor.lockState = (menuOpen) ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void SetColor(int x)
    {
        for (int i=0; i< TroopSelect.Length; i++)
        {
            if (i==x) TroopSelect[i].color = SelectedClr;
            else TroopSelect[i].color = normalClr;
        }
    }

    public void BattleFinish(bool win)
    {
        if (!win) finishDialogue.GetComponent<DialogueBox>().lines[0] = finishDialogue.GetComponent<DialogueBox>().lines[1];
        finishDialogue.SetActive(true);
        ended = true;
    }

    public void ToggleCannonAbility()
    {
        cam1.SetActive(!cam1.activeSelf);
        cam2.SetActive(!cam2.activeSelf);
    }
}
