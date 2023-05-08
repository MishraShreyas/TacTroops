using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetter : MonoBehaviour
{
    private Animator _anim;

    //private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject findClosest()
    {
        string tag = gameObject.tag == "Troop" ? "Enemy" : "Troop";
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);

        GameObject closest = enemies[0];
        float dist = Vector3.Distance(transform.position, closest.transform.position);

        foreach (GameObject x in enemies)
        {
            float d = Vector3.Distance(transform.position, x.transform.position);
            if (d < dist)
            {
                dist = d;
                closest = x;
            }
        }

        return closest;
    }
    
    public void strt(){
        _anim.SetBool("Moving", true);
    }
}
