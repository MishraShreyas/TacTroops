using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour
{
    public GameObject weapon;

    private Animator _anim;


    private bool setting = false;
    private bool begin;

    Stats stats;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();
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
        else if (!gameObject.CompareTag("dead") && begin)
        {
            stats.target = GetComponent<TargetSetter>().findClosest();
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


    
    public void AttackEnter()
    {
        weapon.GetComponent<Collider>().enabled=true;
    }

    public void AttackExit()
    {
        weapon.GetComponent<Collider>().enabled=false;
    }
}
