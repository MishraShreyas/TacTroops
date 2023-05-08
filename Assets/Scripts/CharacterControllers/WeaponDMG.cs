using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDMG : MonoBehaviour
{
    public float dmg;
    public string type;

    AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        if (type=="arrow" || type == "rock") Destroy(gameObject, 5f);
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        string wtag = gameObject.tag != "Tshield" ? "Tweapon" : "Eweapon";
        if (type == "shield" && other.gameObject.tag == "wtag")
        {
            if (other.gameObject.GetComponent<WeaponDMG>().type == "arrow")
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void setDmg(float x)
    {
        dmg = x;
    }

    public void HitSound()
    {
        _as.Play();
    }
}
