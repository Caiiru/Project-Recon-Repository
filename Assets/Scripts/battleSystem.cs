<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public enum BattleState { START,SETTURNS, PLAYERTURN, ENEMYTURN,EOR, WON, LOST}
public class battleSystem : MonoBehaviour
{
    public List<GameObject> chars = new List<GameObject>();
    public BattleState state;

     //--------- BOTOES ----------
    [SerializeField] private bool endTurn = false; //Voltar para a lista e setar o prox turno
    [SerializeField] private bool Reset = false; // resetar se o player e o enemy ja jogaram
    //--------- BOTOES ----------
    //--------- PLAYER ----------
    [SerializeField] private bool playerHasPlayed = false;
    private bool setedPlayerTurn = false;
    [SerializeField] private bool playerAction = false;
    //--------- PLAYER ----------
    //--------- ENEMY -----------
    [SerializeField] private bool enemyHasPlayed = false;
    [SerializeField] private bool enemyAction = false;
    private bool setedEnemyTurn = false;
    //--------- ENEMY -----------

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public BattleHUD playerHud;
    public BattleHUD enemyHUD;
    //Bools para checkar se ja agiu;
    public Text battleStatusText;


    //public bool playerAction = false;  
    //public bool enemyAction = false;
    //private bool comp1Action = false;
    //private bool comp2Action = false;

    void Start()
    {
        state = BattleState.START;
        enemyHUD.setHUD(enemyPrefab.GetComponent<Unit>());
        playerHud.setHUD(playerPrefab.GetComponent<Unit>());
        battleStatusText.text = "Starting Battle";
    }

    private void Update()
    {
        if(endTurn)
        {
            endTurn = false;
            state = BattleState.SETTURNS;
        }
        if(Reset)
        {
            Reset = false;
            playerHasPlayed = false;
            enemyHasPlayed= false;
            setedPlayerTurn = false;
            setedEnemyTurn = false;

        }
        if(playerAction)
        {
            playerAction = false;
            StartCoroutine(PlayerAttack());
            playerHasPlayed = true;
        }
        if(enemyAction)
        {
            enemyAction = false;
            enemyHasPlayed = true;
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
        playerAction = false;
        StartCoroutine(PlayerAttack());
        playerHasPlayed = true;
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

    public void EndOfRound()
    {
        battleStatusText.text = "End Of Round";
        Debug.Log("End of Round");
        //comp1Action = false;
        //comp2Action = false;
        chars.Clear();
        Reset = true;
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
}

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public class battleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject commandsCanvas;
    
    public BattleHUD playerHud;
    public BattleHUD enemyHUD;

    public Text battleStatusText;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        Debug.Log("Starting Battle");
        
        enemyHUD.setHUD(enemyPrefab.GetComponent<Unit>());
        playerHud.setHUD(playerPrefab.GetComponent<Unit>());

        battleStatusText.text = "Starting Battle";

        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        battleStatusText.text = "Your Turn";
        activateCommandsMenu();
    }

    IEnumerator PlayerAttack()
    {
        Debug.Log("Attacking");
        bool isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage);
        enemyHUD.setHP(enemyPrefab.GetComponent<Unit>().currentHP);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {            
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
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
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    public void OnAttackButton()
    {
        Debug.Log("Attack Button");
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else 
        StartCoroutine(PlayerAttack());
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

    public void activateCommandsMenu()
    {
        commandsCanvas.SetActive(true);
    }
}
>>>>>>> nathan
