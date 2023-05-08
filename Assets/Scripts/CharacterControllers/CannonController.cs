using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject shell;
    public GameObject hinge;
    public GameObject smoke;
    public float shootSpeed;
    
    public Vector3 target;
    public float radius=17;

    private Animator _anim;
    private bool begin;
    Vector3 offset = Vector3.zero;
    bool dead;

    Stats stats;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        target = Vector3.zero;
        target.y = Terrain.activeTerrain.SampleHeight(target);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!begin) 
        {
            if (begin != GameManager.instance.strt)
            {
                begin=true;
                shootSpeed = -3*Mathf.Log(stats.speed)+10;
                Debug.Log(shootSpeed);
                StartCoroutine(shoot());
            }
        }

        if (!dead && stats.dead)
        {
            dead=true;
            StopAllCoroutines();
        }
    }

    IEnumerator shoot()
    {
        _anim.SetTrigger("shoot");
        yield return new WaitForSeconds(shootSpeed);
        StartCoroutine(shoot());
    }

    public void shootHit()
    {
        Destroy(Instantiate(smoke, target+ 2*Vector3.up + offset, Quaternion.identity), 2);
        if (gameObject.CompareTag("Enemy"))
        {
            target.x = Random.Range(-50, 50);
            target.z = Random.Range(-50, 50);
            target.y=5;
            target.y = Terrain.activeTerrain.SampleHeight(target);
        }
        offset.x = Random.Range(-3,3);
        offset.z = Random.Range(-3,3);
        Collider[] hitColliders = Physics.OverlapSphere(target + offset, radius);
        string wtag = gameObject.tag != "Troop" ? "Troop" : "Enemy";
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider && hitCollider.CompareTag(wtag) && hitCollider.gameObject.GetComponent<Stats>())
            {
                Debug.Log(hitCollider.gameObject.name+" exploded at " + hitCollider.transform.position);
                hitCollider.gameObject.GetComponent<Stats>().health -= stats.damage;
            }
        }
    }
}
