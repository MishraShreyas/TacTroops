using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardController : MonoBehaviour
{
    public static CardController instance;
    private void Awake() {
        instance=this;
    }

    public Image character;

    public TMP_Text charName;
    public TMP_Text charType;

    public Transform healthFill;
    public Transform speedFill;
    public Transform dmgFill;

    public float maxStat = 25;

    public Sprite[] characters;

    public bool displayOnWake = false;

    public string[] charNames = {"Archer", "Barbarian", "Shield"};
    public string[] charTypes = {"Ranged", "Melee", "Melee"};

    PlayerStats _ps;
    
    // Start is called before the first frame update
    void Start()
    {
        _ps = PlayerStats.instance;
        gameObject.SetActive(displayOnWake);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCard(int type)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        charName.text = charNames[type];
        charType.text = "Type:\n"+charTypes[type];

        character.sprite = characters[type];

        CharacterStats stats = GetStats(charNames[type]);
        float h = stats.health / maxStat;
        float s = stats.speed / maxStat;
        float d = stats.damage / maxStat;

        healthFill.localScale = new Vector3(h,1,1);
        speedFill.localScale = new Vector3(s,1,1);
        dmgFill.localScale = new Vector3(d,1,1);
    }

    CharacterStats GetStats(string type)
    {
        for (int i=0; i<_ps.stats.Length; i++)
        {
            if (_ps.stats[i].type ==  type) return _ps.stats[i];
        }
        return null;
    }
}
