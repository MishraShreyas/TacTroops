using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NailManager : MonoBehaviour
{
    public Image[] nails;

    public int wins;
    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs.SetInt("Lives", 2);
        // PlayerPrefs.SetInt("LivesTut", 1);
        // PlayerPrefs.SetInt("Battles", 8);

        wins = PlayerPrefs.GetInt("Battles");
        ActivateNails();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateNails()
    {
        int n = Mathf.Min(10, wins);
        for (int i=0; i<n; i++)
        {
            nails[i].enabled = true;
            nails[i].color = Color.gray;
        }
    }
}
