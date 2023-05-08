using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject[] rocks;
    public GameObject hand1;
    public GameObject hand2;

    public AudioClip[] audios;

    private Animator _anim;
    Stats stats;
    AudioSource _as;

    bool phase2 = false;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();      
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.target != null && !phase2){
            transform.LookAt(stats.target.transform);
            if (Vector3.Distance(transform.position, stats.target.transform.position) > 7f)
                transform.Translate(Vector3.forward * stats.speed * Time.deltaTime);
            else _anim.SetTrigger("Attack1");

            if (stats.target.CompareTag("dead")) stats.target = null;
        }
        else if (!gameObject.CompareTag("dead"))
        {
            stats.target = GetComponent<TargetSetter>().findClosest();
        }

        if (stats.health <= 95 && !phase2)
        {
            phase2 = true;
            SpecialAttack();
        }
    }

    public void SpecialAttack()
    {
        phase2 = true;
        StartCoroutine(Special());
    }

    public void HitSpecial()
    {
        phase2 = false;
        Vector3 pos = transform.position + Vector3.up*5;
        GameObject tempRock = Instantiate(rocks[0], pos, transform.rotation);
        tempRock.transform.localScale = new Vector3(1,1,1);
        tempRock.transform.Rotate(90, 0, 0);
        tempRock.GetComponent<Outline>().enabled = true;
        tempRock.GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.Impulse);
        Destroy(tempRock, 3);
    }
    
    IEnumerator Special()
    {
        _anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(10);
        phase2 = true;
        StartCoroutine(Special());
    }

    public void PunchEnter()
    {
        hand1.GetComponent<BoxCollider>().enabled = true;
        hand2.GetComponent<BoxCollider>().enabled = true;
    }

    public void PunchExit()
    {
        hand1.GetComponent<BoxCollider>().enabled = false;
        hand2.GetComponent<BoxCollider>().enabled = false;
    }

    public void SpecialRoar()
    {
        _as.PlayOneShot(audios[1]);
    }

    public void NormalRoar()
    {
        _as.PlayOneShot(audios[0]);
    }
}
