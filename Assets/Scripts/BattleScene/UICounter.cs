using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICounter : MonoBehaviour
{
    public string item;
    public int counter;

    private Text t;
    private GameManager _GM;

    [SerializeField]
    bool enab = true;

    void Start()
    {
        t = GetComponent<Text>();
        item = transform.parent.name;
        _GM = GameManager.instance;

        counter = GameManager.instance.getInv(item);
    }

    // Update is called once per frame
    void Update()
    {
        t.text = counter.ToString();

        if (enab && counter<=0)
        {
            enab = false;
            transform.parent.gameObject.GetComponent<Button>().enabled=false;
            transform.parent.gameObject.GetComponent<Image>().color = Color.gray;
        }
    }

}
