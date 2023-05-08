using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    public Image w,s;

    public int currItem=0;
    public StoreGen storeGen;

    AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) NextItem(1);
        if (Input.GetKeyDown(KeyCode.S)) NextItem(-1);

        if (Input.GetKeyDown(KeyCode.W)) w.color = Color.gray;
        if (Input.GetKeyDown(KeyCode.S)) s.color = Color.gray;
        if (Input.GetKeyUp(KeyCode.W)) w.color = Color.white;
        if (Input.GetKeyUp(KeyCode.S)) s.color = Color.white;
    }

    void NextItem(int add)
    {
        currItem += add;
        _as.Play();
        if (currItem>3) currItem=0;
        else if (currItem<0) currItem = 3;

        storeGen.ShowItem(currItem); 
    }

    public void BuyItem()
    {
        storeGen.BuyItem(currItem);
    }
}
