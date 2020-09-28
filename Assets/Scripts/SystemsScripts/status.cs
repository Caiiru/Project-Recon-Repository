using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class status
{   

    private GameObject unidade;
    Unit unit;
    private int enterTurn, currentTurn;

    //Poison
    private bool isPoisoned = false;
    private int poisonTime;
    //Burn
    private bool isBurned = false;
    private int burnTime;
    //Stun
    private bool isStuned = false;

    //Enraizamento
    private bool enraizado  = false;
    private int enraizamentoTimer;



        /* 1- Poison
         * 2- Slow
         * 3- Burn
         * 4- Stun
         * 5- Enraizamento
         */

    public bool generateStatus(int index, int Actualturn,GameObject GO)
    {
        this.enterTurn = Actualturn;

        switch (index)
        {
            case 1:
                if (poisonTime < 6)
                {
                    poisonTime += 3;
                    isPoisoned = true;
                }

                break;

            case 2:


                break;

            case 3:
                if(burnTime < 6)
                {

                    burnTime += 2;
                    isBurned = true;
                }
                break;

            case 4:
                if(isStuned == false)
                {
                    if (GO.GetComponent<Unit>().unitHasPlayed == true)
                    {                        isStuned = true;
                    }
                    else if(GO.GetComponent<Unit>().unitHasPlayed == false)
                    {
                        GO.GetComponent<Unit>().unitHasPlayed = true;
                    }
                }
                
                break;

            case 5:
            if(enraizado == false){

                enraizado = true;
                enraizamentoTimer = 2;
                Debug.Log("ENRAIZADO");
                GO.GetComponent<battleWalk>().disableMoveButton(false);

            }


                break;
        }
        return true;

    }

    public void CheckStatus(GameObject GO)
    {
        if (isPoisoned)
        {
            if (poisonTime > 0)
            {
                poisonTime -= 1;
                GO.GetComponent<Unit>().currentHP -= 3;
            }
            else
                isPoisoned = false;
        }

        if (isBurned)
        {
            if(burnTime > 0)
            {

            }

        }

        if (isStuned)
        {
            GO.GetComponent<Unit>().unitHasPlayed = true;
            isStuned = false;
        }

        if(enraizado){
            if(enraizamentoTimer>0){
                enraizamentoTimer -=1;
                GO.GetComponent<battleWalk>().disableMoveButton(false);
            }
            if(enraizamentoTimer <= 0){
                GO.GetComponent<battleWalk>().disableMoveButton(true);
                enraizado = false;
            }
        }

    }

  


}
