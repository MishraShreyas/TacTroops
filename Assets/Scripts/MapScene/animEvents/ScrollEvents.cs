using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollEvents : MonoBehaviour
{
    public Animator camAnim;
    public GameObject layers;
    public GameObject player;
    public GameObject lives;

    AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextAnim()
    {
        StartCoroutine(NextAnimWrap());
    }

    public void EndAnim()
    {
        camAnim.SetTrigger("next");
    }

    IEnumerator NextAnimWrap()
    {
        layers.SetActive(true);
        yield return new WaitForSeconds(.5f);
        MapGen.instance.GenConnections();
        lives.SetActive(true);
        yield return new WaitForSeconds(.5f);
        player.SetActive(true);

    }

    public void ScrollAudio()
    {
        _as.Play();
    }
}
