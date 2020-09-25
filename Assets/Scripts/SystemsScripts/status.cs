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


    public bool generateStatus(int index, int Actualturn,GameObject GO)
    {
        this.enterTurn = Actualturn;

        switch (index)
        {
            case 1:
                if (poisonTime < 6)
                {
                    Debug.Log("Poisoned");
                    poisonTime += 3;
                    isPoisoned = true;
                }

                break;

            case 2:


                break;

            case 3:
                if(burnTime < 6)
                {

                    Debug.Log("Burning");
                    burnTime += 2;
                    isBurned = true;
                }
                break;

            case 4:
                if(isStuned == false)
                {
                    Debug.Log("Stuned");
                    isStuned = true;
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
                Debug.Log("take damage by poison");
                Debug.Log(poisonTime);
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

    }


}
