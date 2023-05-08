using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopSelect : MonoBehaviour
{
    GameManager _GM;
    UIManager _uim;
    CardController _cc;

    private void Start() 
    {
        _GM = GameManager.instance;
        _uim = UIManager.instance;
        _cc = CardController.instance;
    }

    public void GetCharacter(int x)
    {
        _GM.selected = x;
        _cc.updateCard(x);
        _uim.SetColor(x);
    }
}
