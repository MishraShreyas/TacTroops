using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    [System.Serializable]
    public class classes
    {
        public string Name;
        public GameObject prefab;
    }

    public classes[] prefabs;

    private GameManager _gm;
    private int enems;

    public bool boss = false;
    
    void Start()
    {
        _gm = GameManager.instance;
        int x = PlayerPrefs.GetInt("Map");
        if (boss) x--;
        enems = (int) (2*x*x - x + 1);
        _gm.enemies = enems;
        generator();
    }

    
    void Update()
    {
        
    }

    void generator()
    {
        Vector3 spawnpoint;
        Vector3 add= new Vector3(0,0,5);
        int layer=0;

        int i=0;
        while(i<enems)
        {
            int x = Random.Range(0, prefabs.Length);
            spawnpoint = generateSpawnpt(layer);

            int m = i+3;
            for (int j=i; j<enems; j++)
            {
                if (j==m) break;
                i++;
                spawnpoint.y = Terrain.activeTerrain.SampleHeight(spawnpoint);
                GameObject temp = Instantiate(prefabs[x].prefab, spawnpoint, prefabs[x].prefab.transform.rotation, transform);
                Stats tempStats = temp.GetComponent<Stats>();
                tempStats.health += PlayerPrefs.GetInt("Battles");
                tempStats.speed += PlayerPrefs.GetInt("Battles");
                tempStats.damage += PlayerPrefs.GetInt("Battles");
                spawnpoint += add;
            }
            layer++;
            if (layer>16) layer=0;
        }
    }


    Vector3 generateSpawnpt(int layer)
    {
        float x = -10 + 5*(layer);
        float y = 0;
        float z = Random.Range(-50, 50);
        return new Vector3(x, y, z);
    }
}
