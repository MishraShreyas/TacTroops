using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    public GameObject magic;
    public Transform leftHand;
    public Transform rightHand;

    public GameObject attackCirc;
    public GameObject debuffEffect;

    private Animator _anim;
    public static bool end=false;

    private bool shooting = false;

    private bool begin=false;

    public float radius=10;

    List<GameObject> magics = new List<GameObject>();
    bool magicLoaded = false;

    Stats stats;
    AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();   
        stats = GetComponent<Stats>();
        _as = GetComponent<AudioSource>();
        GenMagic();
    }

    // Update is called once per frame
    void Update()
    {
        begin = GameManager.instance.strt;

        if (stats.target != null){
            transform.LookAt(stats.target.transform);
            if (Vector3.Distance(transform.position, stats.target.transform.position) > 9f)
            {
                transform.Translate(Vector3.forward * stats.speed * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, stats.target.transform.position) < 8f)
            {
                transform.Translate(Vector3.forward * -stats.speed/2 * Time.deltaTime);
            }
            else if (!shooting) StartCoroutine(shoot());

            if (stats.target.CompareTag("dead")) stats.target = null;
        }
        else if (!end && !gameObject.CompareTag("dead") && begin)
        {
            stats.target = GetComponent<TargetSetter>().findClosest();
        }

        if (magicLoaded)
        {
            magics[0].transform.position = leftHand.position;
            magics[1].transform.position = rightHand.position;
        }
    }

    void GenMagic()
    {
        magics.Add(Instantiate(magic, Vector3.zero, Quaternion.identity));
        magics.Add(Instantiate(magic, Vector3.zero, Quaternion.identity));
        magicLoaded = true;
    }

    IEnumerator shoot()
    {
        Debug.Log("shoot");
        shooting = true;
        _anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.1f);
        string tag = gameObject.tag == "Troop" ? "Tweapon" : "Eweapon";
        shooting = false;
    }    

    public void MagicAttack()
    {
        _as.Play();
        Vector3 pos = stats.target.transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(pos);

        Destroy(Instantiate(attackCirc, pos, Quaternion.identity), 1);
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
        string wtag = gameObject.tag != "Troop" ? "Troop" : "Enemy";
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider && hitCollider.CompareTag(wtag) && hitCollider.gameObject.GetComponent<Stats>())
            {
                Debug.Log(hitCollider.gameObject.name+" magicd at " + hitCollider.transform.position);
                hitCollider.gameObject.GetComponent<Stats>().health -= stats.damage;
            }
        }
    }

    public IEnumerator poison(GameObject target)
    {
        for (int i=0 ;i< (int) stats.damage; i++)
        {
            Destroy(Instantiate(attackCirc, target.transform.position, Quaternion.identity), .5f);
            target.GetComponent<Stats>().health -= .5f;
            yield return new WaitForSeconds(.5f);
        }
    }  
        

    public IEnumerator despeedify(GameObject target)
    {
        target.GetComponent<Stats>().speed -= stats.damage;
        Destroy(Instantiate(attackCirc, target.transform.position, Quaternion.identity), .5f);
        yield return new WaitForSeconds(1.5f);
        target.GetComponent<Stats>().speed += stats.damage;
    }

    public IEnumerator dedamagify(GameObject target)
    {
        target.GetComponent<Stats>().damage -= stats.damage;
        Destroy(Instantiate(attackCirc, target.transform.position, Quaternion.identity), .5f);
        yield return new WaitForSeconds(1.5f);
        target.GetComponent<Stats>().damage += stats.damage;
    }
}
