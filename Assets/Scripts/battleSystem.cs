using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { START,SETTURNS, PLAYERTURN, ENEMYTURN,EOR, WON, LOST}
public class battleSystem : MonoBehaviour
{
    public List<GameObject> chars = new List<GameObject>();
    public BattleState state;

     //--------- BOTOES ----------
    private bool endTurn = false; //Voltar para a lista e setar o prox turno
    //--------- PLAYER ----------
    private bool playerHasPlayed = false;
    private bool setedPlayerTurn = false;
    //--------- PLAYER ----------
    //--------- ENEMY -----------
    private bool enemyHasPlayed = false;
    private bool setedEnemyTurn = false;
    //--------- ENEMY -----------

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public BattleHUD playerHud;
    public BattleHUD enemyHUD;
    //Bools para checkar se ja agiu;
    public Text battleStatusText;


    //private bool comp1Action = false;
    //private bool comp2Action = false;
    public GameObject skullHud;
    public GameObject zeroHud;
    public Transform pos1;
    public Transform pos2;
    public GameObject commandsCanvas;


    void Start()
    {
        state = BattleState.START;
        enemyHUD.setHUD(enemyPrefab.GetComponent<Unit>());
        playerHud.setHUD(playerPrefab.GetComponent<Unit>());
        battleStatusText.text = "Starting Battle";
    }

    private void Update()
    {
        if (endTurn)
        {
            state = BattleState.SETTURNS;
        }
        switch (state.ToString())
        {
            case "START":
                CreateLisT();
                 break;
            case "PLAYERTURN":
                    if (setedPlayerTurn == false)
                    { _playerTurn(); }
                    break;
            case "ENEMYTURN":
                    if(setedEnemyTurn == false)
                    { _enemyTurn(); }
                    break;
            case "EOR":
                    EndOfRound();
                    break;
            case "SETTURNS":
                    SetTurns();
                    break;
        }
    }


    void _playerTurn()
    {
        setedPlayerTurn = true;
        activateCommandsMenu();
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
            endTurn = true;
        }
    }
    void _enemyTurn()
    {
        enemyHasPlayed = true;
        setedEnemyTurn = true;
        battleStatusText.text = "Enemy Turn";
        Debug.Log("Enemy turno" );
        
        bool isDead = playerPrefab.GetComponent<Unit>().TakeDamage(enemyPrefab.GetComponent<Unit>().damage);
        playerHud.setHP(playerPrefab.GetComponent<Unit>().currentHP);
        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
            endTurn = true;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else 
        StartCoroutine(PlayerAttack());
        playerHasPlayed = true;
    }

    void EndBattle()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        playerGO.GetComponent<battleWalk>().changeMoveBoolToFalse();
        
        if (state == BattleState.WON)
        {
            battleStatusText.text = "You Win";
        }
        else if (state == BattleState.LOST)
        {
            battleStatusText.text = "You Lose";
        }
    }

    public void EndOfRound()
    {
        battleStatusText.text = "End Of Round";
        Debug.Log("End of Round");
        //comp1Action = false;
        //comp2Action = false;
        chars.Clear();
        playerHasPlayed = false;
        enemyHasPlayed= false;
        setedPlayerTurn = false;
        setedEnemyTurn = false;
        state = BattleState.START;
    } 

    void CreateLisT()
    {
        chars.Add(GameObject.FindGameObjectWithTag("Player"));
        chars.Add(GameObject.FindGameObjectWithTag("Enemy"));
        /*chars.Add(GameObject.FindGameObjectWithTag("Companion1"));
        chars.Add(GameObject.FindGameObjectWithTag("Companion2"));*/
        chars.Add(GameObject.FindGameObjectWithTag("EndOfRound"));
        chars = chars.OrderBy(e => e.GetComponent<Unit>().charSpeed).ToList();
        chars.Reverse();
        SetTurns();
    }
    void SetTurns()
    {
        for (int i = 0; i < chars.Count; i++)
        {
            chars[i].GetComponent<Unit>().listPosition = 0;
            chars[i].GetComponent<Unit>().listPosition = i;
            Debug.Log(chars[i].GetComponent<Unit>().listPosition);
            hudPosition(chars[i].GetComponent<Unit>().listPosition);
        }
        for(int i = 0; i < chars.Count; i++)
        {
            if (chars[i].GetComponent<Unit>().unitName == "Player" && playerHasPlayed == false)
            {
                state = BattleState.PLAYERTURN;
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "Enemy" && enemyHasPlayed == false)
            {
                state =BattleState.ENEMYTURN;
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "EOR")
            {
                state = BattleState.EOR;
                break;
            }
        }
    }  

    void hudPosition(int num)
    {
        switch (num)
        {
            case 0:
                if(chars[0].GetComponent<Unit>().unitName == "Player")
                {
                    print("TAKE THE MEDICINE");
                    skullHud.transform.position = pos1.transform.position;
                }
                else if(chars[0].GetComponent<Unit>().unitName == "Enemy")
                {
                    zeroHud.transform.position = pos1.transform.position;
                }
                print("case 1");
                break;
            case 1:
                if(chars[1].GetComponent<Unit>().unitName == "Player")
                {
                    skullHud.transform.position = pos2.transform.position;
                }
                else if (chars[1].GetComponent<Unit>().unitName == "Enemy")
                {
                    zeroHud.transform.position = pos2.transform.position;
                }
                print("case 2");
                break;

            case 3:
                break;

        }
    }
    
    public void activateCommandsMenu()
    {
        commandsCanvas.SetActive(true);
    }
}
