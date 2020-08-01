using System.Collections;
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