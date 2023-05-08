using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    public Image[] lives;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("Lives", 3);
        if (PlayerPrefs.GetInt("LivesTut") != -1) SetLives();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    void SetLives()
    {
        int life = 3-PlayerPrefs.GetInt("Lives");
        for (int i=0; i<life; i++) lives[i].color = Color.white;
        for (; life<3; life++)
        {
            Color c = Color.white;
            c.a=.07f;
            lives[life].color = c;
        }
    }
}
