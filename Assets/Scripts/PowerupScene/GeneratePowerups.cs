using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePowerups : MonoBehaviour
{
    public List<int> probabilies;
    public List<int> currentCards;
    public List<int> selectedCards;
    public List<Vector4> stats;

    public PowSceneControls pwsc;

    bool ps_load = false;

    PlayerStats _ps;
    // Start is called before the first frame update
    void Start()
    {
        _ps = PlayerStats.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps_load && _ps.loaded)
        {
            GetCurrCards();
            SelectCards();
            GenerateCards();
            pwsc.NextCard(1);
        }
    }

    void GetCurrCards()
    {
        ps_load = true;
        for (int i=0; i<_ps.stats.Length; i++)
        {
            if (_ps.stats[i].quantity>0) currentCards.Add(i);
        }
    }

    void SelectCards()
    {
        if (currentCards.Count == 1)
        {
            for (int i=0; i<3; i++) selectedCards.Add(currentCards[0]);
        }
        else if (currentCards.Count == 2)
        {
            int x = Random.Range(0, 2);

            selectedCards.Add(currentCards[0]);
            selectedCards.Add(currentCards[1]);
            selectedCards.Add(currentCards[x]);
        }
        else
        {
            int x = Random.Range(0, currentCards.Count);
            selectedCards.Add(currentCards[x]);
            while (selectedCards.Count<3)
            {
                x = Random.Range(0, currentCards.Count);
                while (selectedCards.Contains(currentCards[x])) x = Random.Range(0, currentCards.Count);
                selectedCards.Add(currentCards[x]);
            }
        }
    }

    void GenerateCards()
    {
        for (int i=0; i<3; i++)
        {
            Vector4 stat = Vector4.zero;
            int rar = Random.Range(0, probabilies.Count);
            rar = probabilies[rar];
            stat[3]=rar;

            Debug.Log(rar);

            float maxer = rar+1;
            maxer*= 1.5f;

            int sk = Random.Range(0, 3);

            for (int j=0; j<3; j++)
            {
                if (rar==0 && j!=sk) continue;
                if (rar==1 && j==sk) continue;

                stat[j] = Random.Range(0.3f, maxer);
            }

            stats.Add(stat);
        }
    }
}
