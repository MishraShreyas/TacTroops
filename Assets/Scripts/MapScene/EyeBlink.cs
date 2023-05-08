using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour
{
    public Transform eyes;
    public float zStart;
    public float zEnd;
    public float speed;
    bool closing=false;
    bool opening=false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(blink());
        Debug.Log(GameObject.FindObjectsOfType<DialogueBox>().Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (closing)
        {
            eyes.Translate(Vector3.forward * Time.deltaTime * speed);
            if (eyes.localPosition.z >= zEnd)
            {
                closing=false;
                opening = true;
            }
        }
        if (opening)
        {
            eyes.Translate(Vector3.forward * Time.deltaTime * -speed);
            if (eyes.localPosition.z <= zStart)
            {
                opening = false;
            }
        }
    }

    IEnumerator blink()
    {
        yield return new WaitForSeconds(.5f);
        closing = true;
        float wait = Random.Range(3f, 4f);
        yield return new WaitForSeconds(wait);
        StartCoroutine(blink());
    }
}
