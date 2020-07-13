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

    public Transform BattleStation;
    public Transform enemyBattleStation;

    Unit enemyUnit;
    Unit playerUnit;

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
        //Spawnar Prefabs e Imputar os atributos
        GameObject playerGO = playerPrefab;
        //GameObject playerGO = Instantiate(playerPrefab,BattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO =enemyPrefab;
        enemyUnit = enemyGO.GetComponent<Unit>();

        //Setar HUD
        enemyHUD.setHUD(enemyUnit);
        playerHud.setHUD(playerUnit);

        //Setar Text
        
        battleStatusText.text = "Starting Battle";

        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        battleStatusText.text = "Your Turn";
    }

    IEnumerator PlayerAttack()
    {
        Debug.Log("Attacking");
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.setHP(enemyUnit.currentHP);

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

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHud.setHP(playerUnit.currentHP);

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
        if (state == BattleState.WON)
        {
            battleStatusText.text = "You Win";
        }
        else if (state == BattleState.LOST)
        {
            battleStatusText.text = "You Lose";
        }
    }

    public void AttackButton()
    {
        Debug.Log("AttackButton");
    }
}
