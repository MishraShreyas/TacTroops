using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetNewTroops : MonoBehaviour
{
    [System.Serializable]
    public class classes
    {
        public string Name;
        public GameObject prefab;
    }


    public Transform spawn;
    
    public classes[] prefabs;
    public List<GameObject> troops;

    public List<Vector2> numOfTroops;
    public int currT;

    PlayerStats _ps;
    // Start is called before the first frame update
    void Start()
    {
        _ps = PlayerStats.instance;

        GenerateTroop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateTroop()
    {
        List<int> uniques = new List<int>();
        for (int i=0; i<prefabs.Length; i++) uniques.Add(i);

        for (int i=0; i<3; i++)
        {
            int ranNum = uniques[Random.Range(0, uniques.Count)];
            uniques.Remove(ranNum);
            troops.Add(Instantiate(prefabs[ranNum].prefab, spawn.position, spawn.rotation, spawn));
            troops[i].GetComponent<Outline>().OutlineColor = Color.green;
            troops[i].tag = "Troop";
            troops[i].SetActive(false);


            Vector2 temp;
            temp.x = ranNum;
            ranNum = Random.Range(2, Mathf.Min(PlayerPrefs.GetInt("Map")+1, 4));
            temp.y = ranNum;
            numOfTroops.Add(temp);
        }

        currT = -1;
    }

    public void TakeTroops()
    {
        PlayerPrefs.SetInt("ActionComplete", 1);
        foreach (CharacterStats cs in _ps.stats)
        {
            if (cs.type == prefabs[(int)numOfTroops[currT][0]].Name)
            {
                cs.quantity += (int)numOfTroops[currT][1];
                _ps.SaveData();
                SceneManager.LoadScene("Map");
            }
        }
    }
}
