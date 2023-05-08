using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public Button clerAbil;
    public Button wizAbil;
    public Button catAbil;

    bool clEnab = true;
    bool wiEnab = true;
    bool caEnab = true;

    bool clericCdActive=false;
    float clericCd = 5f;
    float clericTimer=0;

    bool wizCdActive=false;
    float wizCd = 5f;
    float wizTimer=0;

    public float radius = 25;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (clericCdActive)
        {
            clericTimer += Time.deltaTime;
            if (clericTimer >= clericCd)
            {
                clericCdActive = false;
                clericTimer = 0;
                clerAbil.enabled=false;
                clerAbil.GetComponent<Image>().color = Color.white;
            }
        }

        
        if (wizCdActive)
        {
            wizTimer += Time.deltaTime;
            if (wizTimer >= wizCd)
            {
                wizCdActive = false;
                wizTimer = 0;
                wizAbil.enabled=true;
                wizAbil.GetComponent<Image>().color = Color.white;
            }
        }

        if (caEnab) CheckCats();
        if (wiEnab) CheckWizards();
        if (clEnab) CheckClerics();
    }

    void CheckClerics()
    {
        int x=0;
        foreach (ClericController cc in FindObjectsOfType<ClericController>())
        {
            if (cc.CompareTag("Troop")) x++;
        }
        if (x<=0)
        {
            clEnab = false;
            clerAbil.enabled = false;
            clerAbil.GetComponent<Image>().color = Color.gray;
        }
    }

    void CheckWizards()
    {
        int x=0;
        foreach (WizardController wc in FindObjectsOfType<WizardController>())
        {
            if (wc.CompareTag("Troop")) x++;
        }
        if (x<=0)
        {
            wiEnab = false;
            wizAbil.enabled = false;
            wizAbil.GetComponent<Image>().color = Color.gray;
        }
    }

    void CheckCats()
    {
        int x=0;
        foreach (CannonController wc in FindObjectsOfType<CannonController>())
        {
            if (wc.CompareTag("Troop")) x++;
        }
        if (x<=0)
        {
            caEnab = false;
            catAbil.enabled = false;
            catAbil.GetComponent<Image>().color = Color.gray;
        }
    }

    public void ClericAbility()
    {
        clericCdActive = true;
        clerAbil.enabled=false;
        clerAbil.GetComponent<Image>().color = Color.gray;
        int b = Random.Range(0,3);
        foreach (ClericController cc in FindObjectsOfType<ClericController>())
        {
            if (cc.gameObject.CompareTag("Troop"))
            {
                buffHelper(cc, b);
            }
        }
    }

    public void WizAbility()
    {
        wizCdActive = true;
        wizAbil.enabled=false;
        wizAbil.GetComponent<Image>().color = Color.gray;
        int b = Random.Range(0,3);
        foreach (WizardController wc in FindObjectsOfType<WizardController>())
        {
            if (wc.gameObject.CompareTag("Troop"))
            {
                debuffHelper(wc, b);
            }
        }
    }

    void buffHelper(ClericController g, int b)
    {
        Collider[] hitColliders = Physics.OverlapSphere(g.transform.position, radius);
        string wtag = "Troop";
        foreach (var hitCollider in hitColliders)
        {
            
            if (hitCollider && hitCollider.CompareTag(wtag) && hitCollider.gameObject.GetComponent<Stats>())
            {
                Debug.Log(hitCollider.gameObject.name+" buffed at " + hitCollider.transform.position);
                switch(b)
                {
                    case 0:
                        StartCoroutine(g.heal(hitCollider.gameObject));
                        break;
                    case 1:
                        StartCoroutine(g.speedify(hitCollider.gameObject));
                        break;
                    case 2:
                        StartCoroutine(g.damagify(hitCollider.gameObject));
                        break;
                    default:
                        break;
                }
                
            }
        }
    }

    public void debuffHelper(WizardController g, int b)
    {
        Collider[] hitColliders = Physics.OverlapSphere(g.transform.position, radius);
        string wtag = "Enemy";
        foreach (var hitCollider in hitColliders)
        {
            
            if (hitCollider && hitCollider.CompareTag(wtag) && hitCollider.gameObject.GetComponent<Stats>())
            {
                Debug.Log(hitCollider.gameObject.name+" debuffed at " + hitCollider.transform.position);
                switch(b)
                {
                    case 0:
                        StartCoroutine(g.poison(hitCollider.gameObject));
                        break;
                    case 1:
                        StartCoroutine(g.despeedify(hitCollider.gameObject));
                        break;
                    case 2:
                        StartCoroutine(g.dedamagify(hitCollider.gameObject));
                        break;
                    default:
                        break;
                }
                
            }
        }
    }
}
