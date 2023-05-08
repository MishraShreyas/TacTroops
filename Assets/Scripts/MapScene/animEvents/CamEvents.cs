using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEvents : MonoBehaviour
{
    public Animator scrollAnim;
    

    Animator selfAnim;
    public bool down=false;

    public string map;
    // Start is called before the first frame update
    void Start()
    {   
        selfAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!down && FindObjectsOfType<DialogueBox>().Length==0)
        {
            StartCoroutine(StartAnim());
        }   
    }

    void NextAnim()
    {
        scrollAnim.SetTrigger("start");

    }

    void EndAnim()
    {
        MapSceneController.instance.LoadScene(map);
    }

    IEnumerator StartAnim()
    {
        down = true;
        yield return new WaitForSeconds(1f);
        selfAnim.SetTrigger("start");
    }
}
