using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClericController : MonoBehaviour
{
    public GameObject weapon;

    private Animator _anim;
    public static bool end=false;

    private bool setting = false;
    private bool begin;

    public GameObject healCirc;
    public GameObject buffCirc;

    public GameObject circ;
    bool circEnab=false;

    Stats stats;
    AudioSource _as;
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
        if (!setting) setWeapon();

        if (stats.target != null){
            transform.LookAt(stats.target.transform);
            if (Vector3.Distance(transform.position, stats.target.transform.position) > 3f)
                transform.Translate(Vector3.forward * stats.speed * Time.deltaTime);
            else _anim.SetTrigger("Attack");

            if (stats.target.CompareTag("dead")) stats.target = null;
        }
        else if (!end && !gameObject.CompareTag("dead") && begin)
        {
            stats.target = GetComponent<TargetSetter>().findClosest();
        }

        if (circEnab)
        {
            float y = Terrain.activeTerrain.SampleHeight(transform.position);
            circ.transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }

    void setWeapon()
    {
        setting = true;
        string tag = gameObject.tag == "Troop" ? "Tweapon" : "Eweapon";
        weapon.tag = tag;
        weapon.layer = LayerMask.NameToLayer(tag);
        weapon.GetComponent<WeaponDMG>().setDmg(stats.damage);
        stats.weapon = weapon;
    }

    public IEnumerator heal(GameObject target)
    {
        _as.Play();
        target.GetComponent<Stats>().health += stats.damage;
        circEnab = true;
        circ = Instantiate(healCirc, Vector3.zero, Quaternion.identity);
        Destroy(circ, 1.5f);
        yield return new WaitForSeconds(1.5f);
        target.GetComponent<Stats>().health -= stats.damage;
        circEnab = false;
        }

    public IEnumerator speedify(GameObject target)
    {
        _as.Play();
        target.GetComponent<Stats>().speed += stats.damage;
        circEnab = true;
        circ = Instantiate(buffCirc, Vector3.zero, Quaternion.identity);
        Destroy(circ , 1.5f);
        yield return new WaitForSeconds(1.5f);
        target.GetComponent<Stats>().speed -= stats.damage;
        circEnab = false;
    }

    public IEnumerator damagify(GameObject target)
    {
        _as.Play();
        target.GetComponent<Stats>().damage += stats.damage;
        circEnab = true;
        circ = Instantiate(buffCirc, Vector3.zero, Quaternion.identity);
        Destroy(circ, 1.5f);
        yield return new WaitForSeconds(1.5f);
        target.GetComponent<Stats>().damage -= stats.damage;
        circEnab = false;
    }

    
    public void AttackEnter()
    {
        weapon.GetComponent<Collider>().enabled=true;
    }

    public void AttackExit()
    {
        weapon.GetComponent<Collider>().enabled=false;
    }
}
