using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreGen : MonoBehaviour
{
    public TMP_Text coinsText;
    public int coins=0;

    public string[] texts;
    public TMP_Text displayTxt;
    public TMP_Text priceTxt;
    public Color goldClr;

    public GameObject endDialogue;
    public Button buyButton;
    public Image enterImg;

    public Transform[] goldFills;
    public Transform[] fills;

    [SerializeField]
    List<int> selected = new List<int>();
    [SerializeField]
    List<int> prices = new List<int>();
    [SerializeField]
    List<bool> bought = new List<bool>();

    bool loaded = false;
    bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        SetCoins();
    }

    // Update is called once per frame
    void Update()
    {
        if (!loaded && PlayerStats.instance.loaded)
        {
            loaded = true;
            GenerateStore();
            ShowItem(0);
            if (CheckWallet()) EndingScene();
        }

        if (end && Input.GetKeyDown(KeyCode.Return))
        {
            end = false;
            SceneManager.LoadScene("Boss");
        }
    }
    

    void SetCoins()
    {
        coins = PlayerPrefs.GetInt("Coins");
        coinsText.text = "x " + coins;
    }

    void GenerateStore()
    {
        List<int> uniques = new List<int>();

        for (int i=0; i<PlayerStats.instance.stats.Length; i++) uniques.Add(i);

        List<int> quant = new List<int>(uniques);

        int x=0;
        for (int i=0; i<uniques.Count; i++)
        {
            x = quant[Random.Range(0, quant.Count)];
            if (PlayerStats.instance.stats[x].quantity==0) quant.Remove(x);
            else break;
        }

        selected.Add(x);
        uniques.Remove(x);
        x = Random.Range(60, 81);
        prices.Add(x);
        bought.Add(false);

        for (int i=1; i<4; i++)
        {
            x = uniques[Random.Range(0, uniques.Count)];
            selected.Add(x);
            uniques.Remove(x);
            x = Random.Range(60, 81);
            prices.Add(x);
            bought.Add(false);
        }

        texts[0] += " (x " + PlayerStats.instance.stats[selected[0]].quantity*2 +")";
    }


    public void ShowItem(int x)
    {
        CardController.instance.updateCard(selected[x]);
        displayTxt.text = texts[x];
        priceTxt.text = prices[x] + " coins";

        for (int i=0; i<3; i++)
        {
            goldFills[i].localScale = fills[i].localScale;
        }

        if (x>0) goldFills[x-1].localScale += Vector3.right * ((float) 5 / CardController.instance.maxStat); 

        if (bought[x] || prices[x] > coins) 
        {
            priceTxt.color = Color.gray;
            displayTxt.color = Color.gray;
            buyButton.interactable = false;
        } else
        {
            priceTxt.color = goldClr;
            displayTxt.color = Color.white;
            buyButton.interactable = true;
        }
    }

    public void BuyItem(int x)
    {
        bought[x]=true;
        switch (x)
        {
            case 0:
                PlayerStats.instance.stats[selected[x]].quantity *= 2;
                break;
            case 1:
                PlayerStats.instance.stats[selected[x]].health += 5;
                break;
            case 2:
                PlayerStats.instance.stats[selected[x]].speed += 5;
                break;
            case 3:
                PlayerStats.instance.stats[selected[x]].damage += 5;
                break;
            default:
                break;
        }
        coins -= prices[x];
        coinsText.text = "x " + coins;
        buyButton.interactable = false;
        
        PlayerStats.instance.SaveData();

        if (CheckWallet()) EndingScene();
    }

    bool CheckWallet()
    {
        int p=0;
        foreach (int price in prices)
        {
            if (coins<price) p++;
        }

        return p==4;
    }

    void EndingScene()
    {
        end = true;
        endDialogue.SetActive(true);
        buyButton.interactable = false;
        enterImg.enabled = true;
        for(int i=0; i<4; i++) bought[i]=true;
    }
}
