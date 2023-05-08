using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject weapon;
    public GameObject shield;

    private Animator _anim;
    public static bool end=false;

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
            if (Vector3.Distance(transform.position, stats.target.transform.position) > 4f)
                transform.Translate(Vector3.forward * stats.speed * Time.deltaTime);
            else _anim.SetTrigger("Attack");

            if (stats.target.CompareTag("dead")) stats.target = null;
        }
        else if (!end && !gameObject.CompareTag("dead") && begin)
        {
            stats.target = GetComponent<TargetSetter>().findClosest();
        }
    }
    
    void setWeapon()
    {
        setting = true;
        string wtag = gameObject.tag == "Troop" ? "Tweapon" : "Eweapon";
        string stag = gameObject.tag == "Troop" ? "Tshield" : "Eshield";

        weapon.tag = wtag;
        weapon.layer = LayerMask.NameToLayer(wtag);
        weapon.GetComponent<WeaponDMG>().setDmg(stats.damage);

        shield.tag = stag;
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
