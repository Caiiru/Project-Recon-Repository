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
    [SerializeField] private bool endTurn = false; //Voltar para a lista e setar o prox turno
    //--------- PLAYER ----------
    private bool playerHasPlayed = false;
    private bool setedPlayerTurn = false;
    private bool suceffulAttack;
    //--------- PLAYER ----------
    //--------- ENEMY -----------
    private bool enemyHasPlayed = false;
    private bool setedEnemyTurn = false;
    //--------- ENEMY -----------
    //--------- COMP1 -----------
    private bool Comp1HasPlayed = false;
    //--------- COMP1 -----------
    //--------- COMP2 -----------
    private bool Comp2HasPlayed = false;
    //--------- COMP2 -----------

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
    public GameObject Comp1Hud;
    public GameObject Comp2Hud;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public c_action acctionC;



    void Start()
    {
        acctionC = acctionC.GetComponent<c_action>();
        state = BattleState.START;
        enemyHUD.setHUD(enemyPrefab.GetComponent<Unit>());
        playerHud.setHUD(playerPrefab.GetComponent<Unit>());
        battleStatusText.text = "Starting Battle";
    }


    private void Update()
    {
        if (acctionC.Retornar())
        {
            acctionC.Resetar();
            
            suceffulAttack = true;

        }
        else if (acctionC.Retornar() == false & acctionC.RetornarErro() == true)
        {
            acctionC.Resetar();
            
            suceffulAttack = false;
        }

        if (endTurn)
        {
            endTurn = false;
            Comp1Hud.transform.localScale = new UnityEngine.Vector3(.3f, .3f);
            Comp2Hud.transform.localScale = new UnityEngine.Vector3(.3f, .3f);
            skullHud.transform.localScale = new UnityEngine.Vector3(.3f, .3f);
            zeroHud.transform.localScale = new UnityEngine.Vector3(.3f, .3f);
            enemyPrefab.GetComponent<TimerForTurn>().Reiniciar();
            playerPrefab.GetComponent<TimerForTurn>().Reiniciar();
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
                if (setedEnemyTurn == false)
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
        playerPrefab.GetComponent<battleWalk>().Commandos.SetActive(true);
        battleStatusText.text = "Your Turn";       
    }
    
    IEnumerator checkAttack(GameObject enemyAttacked)
    {
        yield return new WaitForSeconds(3f);
        playerHasPlayed = true;
        if (suceffulAttack)
        {
            StartCoroutine(PlayerAttack(enemyAttacked));
        }
        else if (suceffulAttack == false)
        {
            endTurn = true;
        }
    }


    IEnumerator PlayerAttack(GameObject enemyAttacked)
    {
        yield return new WaitForSeconds(1f);
        playerPrefab.GetComponent<Unit>().playSound(1);
        
        bool isDead = false;

        if (enemyAttacked.tag == "EnemyPart")
        {
            if (enemyAttacked.GetComponent<Unit>().currentHP <= 0)
            {
                var enemy = enemyAttacked.transform.parent.gameObject;
                isDead = enemy.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage);
                enemy.GetComponent<Unit>().playSound(2);
            }
            else
            {
                enemyAttacked.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage);
                enemyAttacked.GetComponent<Unit>().playSound(2);
            }
        }
        else
        {
            isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage);
            enemyPrefab.GetComponent<Unit>().playSound(2);
        }

        enemyHUD.setHP(enemyPrefab.GetComponent<Unit>().currentHP);

        
        yield return new WaitForSeconds(.5f);

        if (isDead == true)
        {
            enemyPrefab.GetComponent<Unit>().playSound(3);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            Debug.Log("EndTurn");
            endTurn = true;
        }
    }
    void _enemyTurn()
    {
        battleStatusText.text = "Enemy Turn";

        enemyPrefab.GetComponent<TimerForTurn>().Iniciar(2);

        if (enemyPrefab.GetComponent<TimerForTurn>().Sinalizar())
        {
            enemyHasPlayed = true;
            setedEnemyTurn = true;
            
            Debug.Log("Enemy turno");

            enemyPrefab.GetComponent<Unit>().playSound(1);

            bool isDead = playerPrefab.GetComponent<Unit>().TakeDamage(enemyPrefab.GetComponent<Unit>().damage);
            playerHud.setHP(playerPrefab.GetComponent<Unit>().currentHP);

            if (isDead)
            {
                playerPrefab.GetComponent<Unit>().playSound(3);
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                playerPrefab.GetComponent<Unit>().playSound(2);
                endTurn = true;
            }
        }
    }

    public void OnAttackButton(GameObject enemyAttacked)
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
        }
    }

    void EndBattle()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        playerGO.GetComponent<battleWalk>().ChangeMoveBool(false);
        
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
        chars.Clear();
        playerHasPlayed = false;
        enemyHasPlayed= false;
        Comp1HasPlayed = false;
        Comp2HasPlayed = false;
        setedPlayerTurn = false;
        setedEnemyTurn = false;
        state = BattleState.START;
    } 

    void CreateLisT()
    {
        chars.Add(GameObject.FindGameObjectWithTag("Player"));
        chars.Add(GameObject.FindGameObjectWithTag("Enemy"));
        //chars.Add(GameObject.FindGameObjectWithTag("Companion1"));
        //chars.Add(GameObject.FindGameObjectWithTag("Companion2"));
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
                skullHud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.PLAYERTURN;
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "Enemy" && enemyHasPlayed == false)
            {
                zeroHud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.ENEMYTURN;
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "EOR")
            {
                state = BattleState.EOR;
                break;
            }
        }
    }

    public void SkipTurn()
    {
        playerHasPlayed = true;
        SetTurns();
    }

    void hudPosition(int num)
    {
        switch (num)
        {
            case 0:
                if(chars[0].GetComponent<Unit>().unitName == "Player")
                {
                    skullHud.transform.position = pos1.transform.position;
                    Comp1Hud.transform.position = pos3.transform.position;
                }
                else if(chars[0].GetComponent<Unit>().unitName == "Enemy")
                {
                    zeroHud.transform.position = pos1.transform.position;
                    Comp2Hud.transform.position = pos3.transform.position;
                }
                
                break;
            case 1:
                if(chars[1].GetComponent<Unit>().unitName == "Player")
                {
                    skullHud.transform.position = pos2.transform.position;
                    Comp1Hud.transform.position = pos4.transform.position;
                }
                else if (chars[1].GetComponent<Unit>().unitName == "Enemy")
                {
                    zeroHud.transform.position = pos2.transform.position;
                    Comp2Hud.transform.position = pos4.transform.position;
                }
                
                break;

            case 3:
                break;
        }
    }
}
