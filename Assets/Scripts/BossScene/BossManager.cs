using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    private void Awake() {
        instance=this;
    }

    public GameObject bossPrefab;
    public GameObject bossEntryDialogue;
    public GameObject bossHealthBar;
    
    GameObject boss;
    Slider healthBar;
    Stats bossStats;

    public bool bossPhase1 = false;
    public bool bossDead = false;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = bossHealthBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealthBar.activeSelf)
        {
            healthBar.value = bossStats.health;
        }

        if (!bossDead && boss.CompareTag("dead"))
        {
            bossDead  =true;
        }
    }

    public void Begin()
    {
        bossPhase1 = true;
        GameManager.instance.enemies++;
        bossEntryDialogue.SetActive(true);
        bossHealthBar.SetActive(true);
        boss = Instantiate(bossPrefab, bossPrefab.transform.position, bossPrefab.transform.rotation);
        boss.GetComponent<Animator>().SetBool("Moving", true);
        bossStats = boss.GetComponent<Stats>();
    }
}
