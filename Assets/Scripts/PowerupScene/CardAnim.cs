using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnim : MonoBehaviour
{
    public GeneratePowerups powGen;
    public PowSceneControls powCon;

    public float scalex = .1f, scaley = .1f, posy = -125f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCard()
    {
        CardController.instance.updateCard(powGen.selectedCards[powCon.currCard]);
        powCon.UpdateCard();
    }
}
