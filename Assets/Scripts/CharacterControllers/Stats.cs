using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public GameObject target = null;
    public float speed = 5f;
    public float health = 5;
    public float damage = 1;

    public bool dead = false;

    public GameObject weapon;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead && health <= 0) StartCoroutine(die());
    }

    private void OnTriggerEnter(Collider other) {
        string wtag = gameObject.tag != "Troop" ? "Tweapon" : "Eweapon";
        Debug.Log(other.gameObject.tag);
        if (other.CompareTag(wtag))
        {
            WeaponDMG wd = other.gameObject.GetComponent<WeaponDMG>();
            wd.HitSound();
            health -= wd.dmg;
        }
        else if (other.CompareTag("DeleteCharacter"))
        {
            Debug.Log("Deleted " + gameObject.name);
            if (gameObject.CompareTag("Enemy")) GameManager.instance.enemies--;
            else GameManager.instance.troopers--;
            Destroy(gameObject);
        }
    }
    

    IEnumerator die()
    {
        dead = true;
        target = null;
        GetComponent<Outline>().enabled=false;
        if (gameObject.tag == "Enemy") GameManager.instance.enemies--;
        else GameManager.instance.troopers--;
        if (GetComponent<CannonController>() && gameObject.tag == "Troop") UIManager.instance.canons--;

        if (weapon) weapon.tag = "dead";
        gameObject.tag = "dead";
        GetComponent<Animator>().SetTrigger("Dead");

        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }
}
