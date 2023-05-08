using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatUpScript : MonoBehaviour
{
    public int speed;
    public Transform target;
    bool strt = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (strt)
        {
            target.position += Vector3.up * speed * Time.deltaTime;
            if (target.position.y >= 2.5f) strt = false;
        }
    }

    public void begin()
    {
        strt  =true;
    }
}
