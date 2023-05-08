using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverNode : MonoBehaviour
{
    private Renderer _mat;
    private Color _col;

    private bool placed = false;

    private GameManager _gm;
    UIManager _uim;
    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<Renderer>();
        _col = _mat.material.color;
        _gm = GameManager.instance;
        _uim = UIManager.instance;
    }

    

    private void OnMouseEnter() {
        if (placed || !_uim.menuOpen) return;
        Color col = _col;
        col.a = .55f;
        _mat.material.color = col;
    }

    private void OnMouseExit() {
        if (placed || !_uim.menuOpen) return;
        _mat.material.color = _col;
    }

    private void OnMouseDown() {
        if (placed || !_uim.menuOpen) return;
        if (_gm.selected == -1) return;

        if (!_gm.place()) return;

        GameObject troop = Instantiate(_gm.prefabs[_gm.selected].prefab, transform.position, transform.rotation);
        troop.tag = "Troop";
        troop.layer = LayerMask.NameToLayer("Troop");
        troop.GetComponent<Outline>().OutlineColor = Color.green;
        Stats tStats = troop.GetComponent<Stats>();
        SetStats(tStats, _gm.prefabs[_gm.selected].Name);
        Color col = Color.red;
        col.a = .55f;
        _mat.material.color = col;
        placed = true;
    }

    void SetStats(Stats tStats, string type)
    {
        if (type == "Catapult") _uim.canons++;
        foreach (CharacterStats cs in PlayerStats.instance.stats)
        {
            if (cs.type == type)
            {
                tStats.health = cs.health;
                tStats.speed = cs.speed;
                tStats.damage = cs.damage;
            }
        }
    }
}
