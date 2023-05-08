using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TroopSceneControls : MonoBehaviour
{
    public Transform camParent;
    public float speed = 5;

    public Image w,s;
    public TMP_Text numText;
    public TMP_Text about;

    public string[] details;

    bool display=false;
    AudioSource _as;

    public GetNewTroops gnt;
    // Start is called before the first frame update
    void Start()
    {
        camParent = transform;
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) camParent.Rotate(Vector3.up * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) camParent.Rotate(Vector3.up * -speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.W)) ToggleTroop(1);
        if (Input.GetKeyDown(KeyCode.S)) ToggleTroop(-1);

        if (Input.GetKeyDown(KeyCode.W)) w.color = Color.gray;
        if (Input.GetKeyDown(KeyCode.S)) s.color = Color.gray;
        if (Input.GetKeyUp(KeyCode.W)) w.color = Color.white;
        if (Input.GetKeyUp(KeyCode.S)) s.color = Color.white;


        if (!display)
        {
            display=true;
            ToggleTroop(1);
        }
    }

    void ToggleTroop(int add)
    {
        _as.Play();
        gnt.currT += add;
        if (gnt.currT>2) gnt.currT=0;
        else if (gnt.currT<0) gnt.currT = 2;

        for (int i=0; i<3; i++)
        {
            if (i==gnt.currT) gnt.troops[i].SetActive(true);
            else gnt.troops[i].SetActive(false);
        }

        numText.text = ((int)gnt.numOfTroops[gnt.currT][1]).ToString() + "X";
        about.text = details[(int)gnt.numOfTroops[gnt.currT][0]];

        CardController.instance.updateCard((int)gnt.numOfTroops[gnt.currT][0]);
    }
}
