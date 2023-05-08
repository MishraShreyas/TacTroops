using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PowSceneControls : MonoBehaviour
{
    public Image w,s;
    public Image card;
    public TMP_Text rarityTxt;
    public Color[] rarityClr = {Color.green, Color.blue, Color.magenta};
    public string[] rarityStr = {"Common", "Rare", "Epic"};

    public GeneratePowerups powgen;
    public int currCard=-1;

    public Transform[] goldFills;
    public Transform[] fills;
    public Animator cc;

    PlayerStats _ps;
    CardController _cc;

    AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        _ps = PlayerStats.instance;
        _cc = CardController.instance;
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) NextCard(1);
        if (Input.GetKeyDown(KeyCode.S)) NextCard(-1);

        if (Input.GetKeyDown(KeyCode.W)) w.color = Color.gray;
        if (Input.GetKeyDown(KeyCode.S)) s.color = Color.gray;
        if (Input.GetKeyUp(KeyCode.W)) w.color = Color.white;
        if (Input.GetKeyUp(KeyCode.S)) s.color = Color.white;
    }

    public void NextCard(int add)
    {
        _as.Play();
        currCard += add;
        if (currCard>=3) currCard=0;
        else if (currCard<0) currCard=2;

        cc.SetTrigger("pop");
    }

    public void TakePowerup()
    {
        int indx = powgen.selectedCards[currCard];
        _ps.stats[indx].health += powgen.stats[currCard][0];
        _ps.stats[indx].speed += powgen.stats[currCard][1];
        _ps.stats[indx].damage += powgen.stats[currCard][2];
        _ps.SaveData();
        
        PlayerPrefs.SetInt("ActionComplete", 1);
        SceneManager.LoadScene("Map");
    }

    public void UpdateCard()
    {
        int rar = (int)powgen.stats[currCard][3];
        rarityTxt.text = rarityStr[rar];
        card.color = rarityClr[rar];

        _cc.updateCard(powgen.selectedCards[currCard]);
        for (int i=0; i<3; i++)
        {
            float x = powgen.stats[currCard][i] / _cc.maxStat;
            goldFills[i].localScale = fills[i].localScale;
            goldFills[i].localScale += Vector3.right * x;
        }
    }
}
