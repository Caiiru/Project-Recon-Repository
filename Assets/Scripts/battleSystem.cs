using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { START,SETTURNS, PLAYERTURN,COMP1,COMP2, ENEMYTURN,EOR, WON, LOST}
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
            case "COMP1":
                if (setedComp1Turn == false)
                {comp1Turn();}
                break;
            case "COMP2":
                if(setedComp2Turn== false)
                { comp2Turn(); }
                break;
        }
    }
    void comp2Turn()
    {
        setedComp2Turn = true;
        battleStatusText.text = "Companion 2 Turn";
        companion2Prefab.GetComponent<battleWalk>().Commandos.SetActive(true);

    }
    public void OnComp2AttackButton (GameObject enemyAttacked)
    {
        if(state != BattleState.COMP2)
        {
            return;

        }
        else
            Comp2HasPlayed = true;
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
        }
    IEnumerator comp2Attack(GameObject enemyAttacked)
    {
        yield return new WaitForSeconds(1f);
        playerPrefab.GetComponent<Unit>().playSound(1);

        bool isDead = false;

        if (enemyAttacked.tag == "EnemyPart")
        {
            if (enemyAttacked.GetComponent<Unit>().currentHP <= 0)
            {
                var enemy = enemyAttacked.transform.parent.gameObject;
                isDead = enemy.GetComponent<Unit>().TakeDamage(companion2Prefab.GetComponent<Unit>().damage);
                enemy.GetComponent<Unit>().playSound(2);
            }
            else
            {
                enemyAttacked.GetComponent<Unit>().TakeDamage(companion2Prefab.GetComponent<Unit>().damage);
                enemyAttacked.GetComponent<Unit>().playSound(2);
            }
        }
        else
        {
            isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(companion2Prefab.GetComponent<Unit>().damage);
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
    void comp1Turn()
    {
        Debug.Log("Companion 1 Turn");
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
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
    }
    IEnumerator comp1Attack(GameObject enemyAttacked)
    {
        yield return new WaitForSeconds(1f);
        companion1Prefab.GetComponent<Unit>().playSound(1);

        bool isDead = false;

        if (enemyAttacked.tag == "EnemyPart")
        {
            if (enemyAttacked.GetComponent<Unit>().currentHP <= 0)
            {
                var enemy = enemyAttacked.transform.parent.gameObject;
                isDead = enemy.GetComponent<Unit>().TakeDamage(companion1Prefab.GetComponent<Unit>().damage);
                enemy.GetComponent<Unit>().playSound(2);
            }
            else
            {
                enemyAttacked.GetComponent<Unit>().TakeDamage(companion1Prefab.GetComponent<Unit>().damage);
                enemyAttacked.GetComponent<Unit>().playSound(2);
            }
        }
        else
        {
            isDead = enemyPrefab.GetComponent<Unit>().TakeDamage(companion1Prefab.GetComponent<Unit>().damage);
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
        battleStatusText.text = "End Of Round";
        Debug.Log("End of Round");
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

    void CreateLisT()
    {
        chars.Add(GameObject.FindGameObjectWithTag("Player"));
        chars.Add(GameObject.FindGameObjectWithTag("Enemy"));
        chars.Add(GameObject.FindGameObjectWithTag("Companion1"));
        chars.Add(GameObject.FindGameObjectWithTag("Companion2"));
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
            if(chars[i].GetComponent<Unit>().unitName == "Companion 1" && Comp1HasPlayed == false)
            {
                Comp1Hud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.COMP1;
                break;
            }
            if (chars[i].GetComponent<Unit>().unitName == "Companion 2" && Comp2HasPlayed == false)
            {
                Comp2Hud.transform.localScale = new UnityEngine.Vector3(.4f, .4f);
                state = BattleState.COMP2;
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
                endTurn = true;
            break;
            case 1:
                Comp1HasPlayed = true;
                endTurn = true;
                break;
            case 2:
                Comp2HasPlayed = true;
                endTurn = true;
            break;
            case 3:
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
