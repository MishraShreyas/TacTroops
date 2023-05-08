using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class inventory
    {
        public string Name;
        public int quantity;
    }

    [System.Serializable]
    public class classes
    {
        public string Name;
        public GameObject prefab;
    }

    public int selected=-1;

    public int enemies;
    public int troopers=0;

    public classes[] prefabs;
    public inventory[] inv;

    public bool boss=false;

    public bool strt=false;
    private bool fini=false;
    private bool invLoaded=false;

    private UIManager _uim;

    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    private void Start() {
        //enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _uim = UIManager.instance;
        if (boss) PlayerPrefs.SetInt("Lives", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (strt && !fini && (enemies<=0 || troopers <=0))
        {
            if (troopers<=0) boss=false;
            if (!boss){
            fini = true;
            fin(enemies<=0);
            } else if (!BossManager.instance.bossPhase1)
            {
                BossManager.instance.Begin();
                //boss=false;
            } else if (BossManager.instance.bossDead)
            {
                fini = true;
                fin(true);
            }
        }

        if (!invLoaded && PlayerStats.instance.loaded) LoadInv();
    }

    public void LoadInv()
    {
        invLoaded = true;
        foreach (CharacterStats cs in PlayerStats.instance.stats)
        {
            foreach (inventory i in inv)
            {
                if (i.Name == cs.type) i.quantity = cs.quantity;
            }
        }
    }

    public void begin()
    {
        strt=true;

        foreach (TargetSetter t in FindObjectsOfType<TargetSetter>())
        {
            t.strt();
        }

        foreach (HoverNode h in FindObjectsOfType<HoverNode>())
        {
            h.gameObject.SetActive(false);
        }
    }

    void fin(bool win)
    {
        strt = false;

        StartCoroutine(endgame(win));
    }

    public int getInv(string s)
    {
        for (int i=0; i<inv.Length; i++)
        {
            if (inv[i].Name == s)
            {
                return inv[i].quantity;
            }
        }

        return 0;
    }

    public bool place()
    {
        foreach (UICounter g in FindObjectsOfType<UICounter>())
        {
            if (g.item == prefabs[selected].Name)
            {
                if (g.counter<=0) return false;
                g.counter--;
                troopers++;
                return true;
            }
        }

        return true;
    }

    IEnumerator endgame(bool win)
    {
        FreeFlyCamera.instance._active = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetInt("ActionComplete", 1);
        if (win)
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins += troopers-1;
            int bs = PlayerPrefs.GetInt("Battles");
            PlayerPrefs.SetInt("Battles", bs+1);
            if (PlayerPrefs.GetInt("CoinsTut") == -troopers) PlayerPrefs.SetInt("CoinsTut", 0);
            PlayerPrefs.SetInt("Coins", coins);
        } else 
        {
            int lives = PlayerPrefs.GetInt("Lives");
            PlayerPrefs.SetInt("Lives", lives-1);
            if (lives==3) PlayerPrefs.SetInt("LivesTut", 0);
        }
        _uim.BattleFinish(win);
    }
}
