using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    public Image hex;
    public Image eventImg;
    public Sprite fadedHex;
    public string type="Troop";
    public bool faded=false;

    public UnityEngine.UI.Outline _ol;
    public float speed = .5f;
    // Start is called before the first frame update
    void Start()
    {
        hex = GetComponent<Image>();
        _ol = GetComponent<UnityEngine.UI.Outline>();
        //GrayOut();
        if (MapGen.instance.currLayer<4 && MapGen.instance.grid[MapGen.instance.currLayer].Contains(gameObject) && !faded)
        {
            StartCoroutine(flash());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrayOut()
    {
        faded=true;
        hex.sprite = fadedHex;
        hex.color = Color.white;
        GetComponent<Button>().enabled = false;
    }

    public void LoadType(string s)
    {
        type = s;
        switch (s)
        {
            case "Troop":
                eventImg.sprite = MapGen.instance.events[0];
                break;
            case "Powerup":
                eventImg.sprite = MapGen.instance.events[1];
                break;
            case "Battle":
                eventImg.sprite = MapGen.instance.events[2];
                break;
            default: break;
        }
    }

    public IEnumerator flash()
    {
        yield return new WaitForSeconds(speed);

        _ol.enabled = true;

        yield return new WaitForSeconds(speed);

        _ol.enabled = false;

        StartCoroutine(flash());
    }
}
