using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    public GameObject arrow;
    public Transform arrowPoint;

    private Animator _anim;
    public static bool end=false;
    private bool shooting = false;

    private bool begin;
    AudioSource _as;
    
    Stats stats;
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
        begin = GameManager.instance.strt;

        if (stats.target != null){
            transform.LookAt(stats.target.transform);
            if (Vector3.Distance(transform.position, stats.target.transform.position) > 9f)
            {
                _anim.SetBool("Back", false);
                transform.Translate(Vector3.forward * stats.speed * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, stats.target.transform.position) < 8f)
            {
                _anim.SetBool("Back", true);
                transform.Translate(Vector3.forward * -stats.speed / 2 * Time.deltaTime);
            }
            else if (!shooting) StartCoroutine(shoot());

            if (stats.target.CompareTag("dead")) stats.target = null;
        }
        else if (!end && !gameObject.CompareTag("dead") && begin)
        {
            stats.target = GetComponent<TargetSetter>().findClosest();
        }
    }

    IEnumerator shoot()
    {
        _as.Play();
        shooting = true;
        _anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(1.1f);
        GameObject shot = Instantiate(arrow, arrowPoint.position - Vector3.right*3, transform.rotation);
        shot.GetComponent<WeaponDMG>().setDmg(stats.damage);
        string tag = gameObject.tag == "Troop" ? "Tweapon" : "Eweapon";
        shot.tag = tag;
        shot.layer = LayerMask.NameToLayer(tag);
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * 150f, ForceMode.Impulse);
        shooting = false;
    }

}
