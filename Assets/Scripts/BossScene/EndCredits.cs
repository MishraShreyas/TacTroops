using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Lives", 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (transform.position.y >= 1900 || Input.GetKeyDown(KeyCode.Space))
        {
            End();
        }
    }

    public void End()
    {
        SceneManager.LoadScene(0);
    }
}
