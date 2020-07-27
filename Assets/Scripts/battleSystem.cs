using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START,CHECKSPEED,SETTURNS,ENDOFROUND, PLAYERTURN, ENEMYTURN, WON, LOST}
public class battleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public BattleHUD playerHud;
    public BattleHUD enemyHUD;

    public Text battleStatusText;

    private string turnOrder;


    //Bools para checkar se ja agiu;
    public bool playerAction = false;  
    public bool enemyAction = false;
    //private bool comp1Action = false;
    //private bool comp2Action = false;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        if (state == BattleState.CHECKSPEED)
        {
            
        }
    }
    IEnumerator SetupBattle()
    {
        

        //Setar HUD
        enemyHUD.setHUD(enemyPrefab.GetComponent<Unit>());
        playerHud.setHUD(playerPrefab.GetComponent<Unit>());

        //Setar Text
        
        battleStatusText.text = "Starting Battle";

        yield return new WaitForSeconds(.5f);
        state = BattleState.CHECKSPEED;
        SpeedCheck();
        
    }

    private void PlayerTurn()
    {
        battleStatusText.text = "Your Turn";
    }

    IEnumerator PlayerAttack()
    { 
        bool isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage);
        enemyHUD.setHP(enemyPrefab.GetComponent<Unit>().currentHP);

        yield return new WaitForSeconds(.5f);

        if (isDead == true)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            
            playerAction = true;
            Debug.Log(turnOrder);
            state = BattleState.SETTURNS;
            SetTurns(turnOrder);
        }
    }
    IEnumerator EnemyTurn()
    {
        battleStatusText.text = "Enemy Turn";

        yield return new WaitForSeconds(1f);

        bool isDead = playerPrefab.GetComponent<Unit>().TakeDamage(enemyPrefab.GetComponent<Unit>().damage);
        playerHud.setHP(playerPrefab.GetComponent<Unit>().currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            enemyAction = true;
            state = BattleState.SETTURNS;
            SetTurns(turnOrder);
            
        }


    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else 
        StartCoroutine(PlayerAttack());
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            battleStatusText.text = "You Win";
        }
        else if (state == BattleState.LOST)
        {
            battleStatusText.text = "You Lose";
        }
    }

    void SpeedCheck()
    {
        Debug.Log("Check");

        gameObject.GetComponent<speedSort>().SortList(); 

        if (gameObject.GetComponent<speedSort>().personagens[0] == GameObject.FindGameObjectWithTag("Enemy"))
        {
            if (gameObject.GetComponent<speedSort>().personagens[1] == GameObject.FindGameObjectWithTag("Player"))
            {
                if (gameObject.GetComponent<speedSort>().personagens[2] == GameObject.FindGameObjectWithTag("Companion1"))
                {
                    if (gameObject.GetComponent<speedSort>().personagens[3] == GameObject.FindGameObjectWithTag("Companion2"))
                    {
                        turnOrder = "EPC1C2";
                    }
                }
            }
        }//EPC1C2
        if (gameObject.GetComponent<speedSort>().personagens[0] == GameObject.FindGameObjectWithTag("Enemy"))
        {
            if (gameObject.GetComponent<speedSort>().personagens[1] == GameObject.FindGameObjectWithTag("Companion2"))
            {
                if(gameObject.GetComponent<speedSort>().personagens[2] == GameObject.FindGameObjectWithTag("Companion1"))
                {
                    if (gameObject.GetComponent<speedSort>().personagens[3] == GameObject.FindGameObjectWithTag("Player"))
                    {
                        turnOrder = "EC2C1P";
                    }
                }
            }
        }//EC2C1P


        //Debug.Log(turnOrder);
        state = BattleState.SETTURNS;
        SetTurns(turnOrder);
    }


    void SetTurns(string turnOrder) // Arruma os turnos conforme o codigo turnOrder dado no SpeedCheck
    {
        if (turnOrder == "EPC1C2")
        {
            if (enemyAction == false)
            {
                EnemyTurn();
                state = BattleState.ENEMYTURN;
            }

            else
            {
                PlayerTurn();
                state = BattleState.PLAYERTURN;
            }

        }
        if (turnOrder == "EC2C1P")
        {
            if (enemyAction == false)
            {
                StartCoroutine(EnemyTurn());
                state = BattleState.ENEMYTURN;
            }

            else if(playerAction == false)
            {
                PlayerTurn();
                state = BattleState.PLAYERTURN;
            }

            else
            {
                EndOfRound();
                
            }

        }
    }

    void EndOfRound()
    {
        battleStatusText.text = "End Of Round";
        state = BattleState.ENDOFROUND;
        Debug.Log("End of Round");
        enemyAction = false;
        playerAction = false;
        //comp1Action = false;
        //comp2Action = false;
        gameObject.GetComponent<speedSort>().DeleteList();

        SpeedCheck();
    } // reseta tudo e joga para o SpeedCheck criar uma lista nova

    void Companion1Turn()
    {

    }
}