using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { START,SETTURNS, PLAYERTURN,COMP1,COMP2, ENEMYTURN,EOR, WON, LOST,NOTBATTLE}

public class battleSystem : MonoBehaviour
{
    public List<GameObject> chars = new List<GameObject>();
    public BattleState state;

    public bool Poison;

    //--------- BOTOES ----------
    [SerializeField] private bool endTurn = false; //Voltar para a lista e setar o prox turno

    //--------- PLAYER ----------
    private bool playerHasPlayed = false;
    private bool setedPlayerTurn = false;

    private bool suceffulAttack;

    //--------- PLAYER ----------
    //--------- ENEMY -----------
    private bool enemyHasPlayed = false;

    [SerializeField] private bool setedEnemyTurn = false;

    //--------- ENEMY -----------
    //--------- COMP1 -----------
    private bool setedComp1Turn = false;

    private bool Comp1HasPlayed = false;

    //--------- COMP1 -----------
    //--------- COMP2 -----------
    private bool setedComp2Turn = false;

    private bool Comp2HasPlayed = false;
    //--------- COMP2 -----------

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject companion1Prefab;
    public GameObject companion2Prefab;

    public BattleHUD playerHud;
    public BattleHUD enemyHUD;

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

    public GameObject commandsCanvas;
    public GameObject comp1CommandsCanvas;
    public GameObject comp2CommandsCanvas;

    public c_action acctionC;

    private bool timerForEnemyTurn;



    void Start()
    {
        acctionC = acctionC.GetComponent<c_action>();
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
                {
                    _playerTurn();
                }

                break;
            case "ENEMYTURN":
                _enemyTurn();
                break;
            case "EOR":
                EndOfRound();
                break;
            case "SETTURNS":
                SetTurns();
                break;
            case "COMP1":
                if (setedComp1Turn == false)
                {
                    comp1Turn();
                }

                break;
            case "COMP2":
                if (setedComp2Turn == false)
                {
                    comp2Turn();
                }

                break;
        }
    }

    public void changeStateToStart()
    {
        state = BattleState.START;
    }

    void comp2Turn()
    {
        setedComp2Turn = true;
        battleStatusText.text = "Companion 2 Turn";
        companion2Prefab.GetComponent<battleWalk>().Commandos.SetActive(true);
    }

    public void OnComp2AttackButton(GameObject enemyAttacked)
    {
        if (state != BattleState.COMP2)
        {
            return;

        }
        else
        {
            Comp2HasPlayed = true;
            companion2Prefab.GetComponent<Unit>().unitHasPlayed = true;
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
        }
    }

    IEnumerator comp2Attack(GameObject enemyAttacked)
    {
        companion2Prefab.GetComponent<Unit>().isAttacking();
        yield return new WaitForSeconds(1f);
        playerPrefab.GetComponent<Unit>().playSound(1);

        bool isDead = false;

        if (enemyAttacked.tag == "EnemyPart")
        {
            if (enemyAttacked.GetComponent<Unit>().currentHP <= 0)
            {
                var enemy = enemyAttacked.transform.parent.gameObject;
                isDead = enemy.GetComponent<Unit>().TakeDamage(companion2Prefab.GetComponent<Unit>().damage, companion2Prefab.GetComponent<Unit>().element);
                enemy.GetComponent<Unit>().playSound(2);
            }
            else
            {
                enemyAttacked.GetComponent<Unit>().TakeDamage(companion2Prefab.GetComponent<Unit>().damage, companion2Prefab.GetComponent<Unit>().element);
                enemyAttacked.GetComponent<Unit>().playSound(2);
            }
        }
        else
        {
            isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(companion2Prefab.GetComponent<Unit>().damage, companion2Prefab.GetComponent<Unit>().element);
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
            companion2Prefab.GetComponent<Unit>().unitHasPlayed = true;
            endTurn = true;
        }
    }
    void comp1Turn()
    {
        setedComp1Turn = true;
        battleStatusText.text = "Companion 1 Turn";
        companion1Prefab.GetComponent<battleWalk>().Commandos.SetActive(true);
    }

    public void OnComp1AttackButton (GameObject enemyAttacked)
    {
        if(state != BattleState.COMP1)
        {
            return;

        }
        else
            Comp1HasPlayed = true;
            companion1Prefab.GetComponent<Unit>().unitHasPlayed = true;
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
    }
    IEnumerator comp1Attack(GameObject enemyAttacked)
    {
        companion1Prefab.GetComponent<Unit>().isAttacking();
        yield return new WaitForSeconds(.1f);
        companion1Prefab.GetComponent<Unit>().playSound(1);

        bool isDead = false;

        if (enemyAttacked.tag == "EnemyPart")
        {
            if (enemyAttacked.GetComponent<Unit>().currentHP <= 0)
            {
                var enemy = enemyAttacked.transform.parent.gameObject;
                isDead = enemy.GetComponent<Unit>().TakeDamage(companion1Prefab.GetComponent<Unit>().damage,companion1Prefab.GetComponent<Unit>().element);
                enemy.GetComponent<Unit>().playSound(2);
            }
            else
            {
                enemyAttacked.GetComponent<Unit>().TakeDamage(companion1Prefab.GetComponent<Unit>().damage, companion1Prefab.GetComponent<Unit>().element);
                enemyAttacked.GetComponent<Unit>().playSound(2);
            }
        }
        else
        {
            isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(companion1Prefab.GetComponent<Unit>().damage, companion1Prefab.GetComponent<Unit>().element);
            enemyPrefab.GetComponent<Unit>().playSound(2);
        }

        enemyHUD.setHP(enemyPrefab.GetComponent<Unit>().currentHP);


        yield return new WaitForSeconds(.1f);

        if (isDead == true)
        {
            enemyPrefab.GetComponent<Unit>().playSound(3);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            companion1Prefab.GetComponent<Unit>().unitHasPlayed = true;
            endTurn = true;
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
        yield return new WaitForSeconds(2.7f);
        
        if (suceffulAttack & state==BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack(enemyAttacked));
        }
        else if(suceffulAttack & state == BattleState.COMP1)
        {
            StartCoroutine(comp1Attack(enemyAttacked));
        }
        else if(suceffulAttack & state == BattleState.COMP2)
        {
            StartCoroutine(comp2Attack(enemyAttacked));
        }
        else if (suceffulAttack == false)
        {
            endTurn = true;
        }
    }

    IEnumerator PlayerAttack(GameObject enemyAttacked)
    {
        
        playerPrefab.GetComponent<Unit>().isAttacking();
        yield return new WaitForSeconds(.1f);
        playerPrefab.GetComponent<Unit>().playSound(1);
        
        bool isDead = false;

        if (enemyAttacked.tag == "EnemyPart")
        {
            if (enemyAttacked.GetComponent<Unit>().currentHP <= 0)
            {
                var enemy = enemyAttacked.transform.parent.gameObject;
                isDead = enemy.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage, playerPrefab.GetComponent<Unit>().element);
                enemy.GetComponent<Unit>().playSound(2);
            }
            else
            {
                enemyAttacked.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage, playerPrefab.GetComponent<Unit>().element);
                enemyAttacked.GetComponent<Unit>().playSound(2);
            }
        }
        else
        {
            isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(playerPrefab.GetComponent<Unit>().damage, playerPrefab.GetComponent<Unit>().element);
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
            endTurn = true;
        }
    }
    void _enemyTurn()
    {
        battleStatusText.text = "Enemy Turn";

        if (setedEnemyTurn == false)
        {
            if (timerForEnemyTurn == false)
            {
                Debug.Log("Inicializano");
                enemyPrefab.GetComponent<TimerForTurn>().Reiniciar();
                enemyPrefab.GetComponent<TimerForTurn>().Iniciar(1);
                timerForEnemyTurn = true;
            }

            if (enemyPrefab.GetComponent<TimerForTurn>().Sinalizar())
            {
                Debug.Log("Sinalizzsado");
                
                setedEnemyTurn = true;
                var epChange = enemyPrefab.GetComponent<EnemyBattleWalk>();
                epChange.ChangeCanAct(true);
                enemyPrefab.GetComponent<EnemyBattleWalk>().ChangeEndTurn(false);
            }
        }

        //        Debug.Log("Enemy turno");
        int KKK = 20;

        if (enemyPrefab.GetComponent<EnemyBattleWalk>().ReturnEndTurn() && setedEnemyTurn && timerForEnemyTurn)
        
        {
            Debug.Log("ENEMY HAS PLAYED");
            enemyHasPlayed = true;

            if (playerPrefab.GetComponent<Unit>().currentHP <= 0)
            {
                Debug.Log("BATTLE LOST");
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                Debug.Log("ENDING TURN " + endTurn);
                endTurn = true;
                timerForEnemyTurn = false;
                enemyPrefab.GetComponent<Unit>().unitHasPlayed = true; 
                state = BattleState.EOR;
            }
        }
    }

    public void OnAttackButton(GameObject enemyAttacked)
    {
        if (state == BattleState.COMP1)
        {
            OnComp1AttackButton(enemyAttacked);
        }
        else if(state == BattleState.COMP2)
        {
            OnComp2AttackButton(enemyAttacked);
        }
        else
        {
            playerHasPlayed = true;
            playerPrefab.GetComponent<Unit>().unitHasPlayed = true;
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
        }
    }

    void EndBattle()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        playerGO.GetComponent<battleWalk>().ChangeMoveBool(false);
        var sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        
        if (state == BattleState.WON)
        {
            sceneManager.GetComponent<sceneController>().endBattle("Win");
            battleStatusText.text = "You Win";
        }
        else if (state == BattleState.LOST)
        {
            battleStatusText.text = "You Lose";
            sceneManager.GetComponent<sceneController>().endBattle("Lose");
        }
    }

    public void EndOfRound()
    {
        for (int i = 0; i < chars.Count; i++)
        {
            chars[i].GetComponent<Unit>().unitHasPlayed = false;
            chars[i].GetComponent<Unit>().currentTurn += 1;
            chars[i].GetComponent<Unit>().CheckStatus();
        }
        battleStatusText.text = "End Of Round";
        chars.Clear();
        playerHasPlayed = false;
        enemyHasPlayed= false;
        Comp1HasPlayed = false;
        Comp2HasPlayed = false;
        setedComp1Turn = false;
        setedComp2Turn = false;
        setedPlayerTurn = false;
        setedEnemyTurn = false;
        state = BattleState.START;
    } 

    private void CreateLisT()
    {
        var go = gameObject;

        chars.Add(GameObject.FindGameObjectWithTag("Player"));
        chars.Add(GameObject.FindGameObjectWithTag("Enemy"));
        
        go = GameObject.FindGameObjectWithTag("Companion1");
        if(go != null){
               chars.Add(go);
        }
        
        go = GameObject.FindGameObjectWithTag("Companion2");
        if(go != null){
            chars.Add(go);
        }
        
        chars.Add(GameObject.FindGameObjectWithTag("EndOfRound"));
        for (int i = 0; i<chars.Count; i++)
        {
            chars[i].GetComponent<Unit>().totalSpeed += chars[i].GetComponent<Unit>().charSpeed;
            
        }
        chars = chars.OrderBy(e => e.GetComponent<Unit>().totalSpeed).ToList();
        chars.Reverse();
        SetTurns();
    }
    void SetTurns()
    {
        for (int i = 0; i < chars.Count; i++)
        {
            chars[i].GetComponent<Unit>().listPosition = 0;
            chars[i].GetComponent<Unit>().listPosition = i;
            hudPosition(chars[i].GetComponent<Unit>().listPosition);
        }
        for(int i = 0; i < chars.Count; i++)
        {
            if (chars[i].GetComponent<Unit>().unitName == "Player" && chars[i].GetComponent<Unit>().unitHasPlayed == false)
            {
                skullHud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.PLAYERTURN;
                //Debug.Log("Player Turn");
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "Enemy" && chars[i].GetComponent<Unit>().unitHasPlayed == false)
            {
                zeroHud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.ENEMYTURN;
                //Debug.Log("Enemuy Turn");
                break;
            }
            if(chars[i].GetComponent<Unit>().unitName == "Companion 1" && chars[i].GetComponent<Unit>().unitHasPlayed == false)
            {
                Comp1Hud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.COMP1;
                //Debug.Log("Companio Turn");
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "Companion 2" && chars[i].GetComponent<Unit>().unitHasPlayed == false)
            {
                Comp2Hud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.COMP2;
               //Debug.Log("Companion2 Turn");
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "EOR")
            {
                state = BattleState.EOR;
                break;
            }
        }
    }

    public void SkipTurn(int entity)
    {
        switch (entity)
        {
            case 0:
                playerHasPlayed = true;
            playerPrefab.GetComponent<Unit>().unitHasPlayed = true;
                endTurn = true;
            break;
            case 1:
                Comp1HasPlayed = true;
            companion1Prefab.GetComponent<Unit>().unitHasPlayed = true;
                endTurn = true;
                break;
            case 2:
            companion2Prefab.GetComponent<Unit>().unitHasPlayed = true;
                Comp2HasPlayed = true;
                endTurn = true;
            break;
            case 3:
            enemyPrefab.GetComponent<Unit>().unitHasPlayed = true;
                enemyHasPlayed = true;
                endTurn = true;
            break;
        }
        
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
                }
                else if(chars[0].GetComponent<Unit>().unitName == "Enemy")
                {
                    zeroHud.transform.position = pos1.transform.position;
                }
                else if(chars[0].GetComponent<Unit>().unitName == "Companion 1")
                {
                    Comp1Hud.transform.position = pos1.transform.position;
                }
                else if(chars[0].GetComponent<Unit>().unitName == "Companion 2")
                {
                    Comp2Hud.transform.position = pos1.transform.position;
                }
                
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
                else if(chars[1].GetComponent<Unit>().unitName == "Companion 1")
                {
                    Comp1Hud.transform.position = pos2.transform.position;
                }
                else if(chars[1].GetComponent<Unit>().unitName == "Companion 2")
                {
                    Comp2Hud.transform.position = pos2.transform.position;
                }
                
                break;

            case 2:
                if(chars[2].GetComponent<Unit>().unitName == "Companion 1")
                {
                    Comp1Hud.transform.position = pos3.transform.position;
                }
                else if(chars[2].GetComponent<Unit>().unitName == "Companion 2")
                {
                    Comp2Hud.transform.position = pos3.transform.position;
                }
                else if(chars[2].GetComponent<Unit>().unitName == "Player")
                {
                    skullHud.transform.position = pos3.transform.position;
                }
                else if(chars[2].GetComponent<Unit>().unitName == "Enemy")
                {
                    zeroHud.transform.position = pos3.transform.position;
                }
                break;
            case 3:
                if(chars[3].GetComponent<Unit>().unitName == "Companion 2")
                {
                    Comp2Hud.transform.position = pos4.transform.position;
                }
                else if(chars[3].GetComponent<Unit>().unitName == "Companion 1")
                {
                    Comp2Hud.transform.position = pos4.transform.position;
                }
                else if(chars[3].GetComponent<Unit>().unitName == "Player")
                {
                    skullHud.transform.position = pos4.transform.position;
                }
                else if(chars[3].GetComponent<Unit>().unitName == "Enemy")
                {
                    zeroHud.transform.position = pos4.transform.position;
                }
                break;
        }
    }
}
