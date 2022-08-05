using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { START,SETTURNS, PLAYERTURN,COMP1,COMP2, ENEMYTURN,EOR, WON, LOST,NOTBATTLE}

public class battleSystem : MonoBehaviour
{
    public ShowNextUnitTurn ShowNextUnitTurn;
    
    public List<GameObject> chars = new List<GameObject>();
    public BattleState state;

    public bool Poison;

    //--------- BOTOES ----------
    [SerializeField] private bool endTurn = false; //Voltar para a lista e setar o prox turno

    //--------- PLAYER ----------
    private bool playerHasPlayed = false;
    private bool setedPlayerTurn = false;

    private bool successfullAttack;

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

    private GameObject mainCamera, menuPanel;
    
    private Vector3[] cameraTravelPoints;

    private Vector3 target;

    private int pointsIndex;

    private bool timerForEnemyTurn, initialFade, outOfMenu, moveCam = true, playerDealtDamage, comp1DealtDamage;
    
    private bool comp2DealtDamage, lost, won, canvasResetSuccessfully, enemyWasAttacked;
    
    public GameObject battleStatus, turnHud;

    private FadeImageCode fadeImageCode;

    public int DamageDealtValueToReset, DamageTakenValueToReset;

    private GameObject _attackTarget;

    private CheckAnimationState unitAnimationState;

    private bool needsToEndTurn, needsToSkipTurn;

    private int entityToEndTurn, entityToSkipTurn;

    void Start()
    {
        BattleRating.Companion1Alive = true;
        BattleRating.Companion2Alive = true;
        BattleRating.DamageDealt = DamageDealtValueToReset;
        BattleRating.DamageTaken = DamageTakenValueToReset;
        
        acctionC = acctionC.GetComponent<c_action>();
        battleStatusText.text = "Starting Battle";
        mainCamera = GameObject.Find("Main Camera");
        var travelPoints = GameObject.FindGameObjectsWithTag("CameraTravelPoints");
        cameraTravelPoints = new Vector3[travelPoints.Length];
        for (int x = 0; x < travelPoints.Length; x++)
        {
            cameraTravelPoints[x] = travelPoints[x].transform.position;
        }
        
        menuPanel = GameObject.Find("MenuPanel");
        fadeImageCode = GameObject.Find("FadeImage").GetComponent<FadeImageCode>();
    }


    private void Update()
    {
        if (initialFade)
        {
            if (outOfMenu)
            {
                if (needsToEndTurn && CheckForAllUnitsAnimation())
                {
                    EndingTurn();
                }
                else if(needsToSkipTurn && CheckForAllUnitsAnimation())
                {
                    SkippingTurn();
                }
                else
                {
                    if (mainCamera.transform.position != new Vector3(-0.51f, 0.34f, -10))
                    {
                        mainCamera.transform.position = new Vector3(-0.51f, 0.34f, -10);
                    }

                    if (acctionC.IsActive)
                    {
                        if (acctionC.Retornar())
                        {
                            successfullAttack = true;
                            acctionC.TurnOffHud();
                        }
                        else if (acctionC.RetornarErro())
                        {
                            successfullAttack = false;
                            acctionC.TurnOffHud();
                        }
                    }

                    if (enemyWasAttacked && canvasResetSuccessfully == false)
                    {
                        Debug.Log("RESETTING COMMANDS CANVAS!!!!");
                        ResetCommandCanvas();
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
                        playerDealtDamage = false;
                        comp1DealtDamage = false;
                        comp2DealtDamage = false;

                        state = BattleState.SETTURNS;
                    }
                    
                    switch (state.ToString())
                    {
                        case "START":
                            CreateLisT();
                            break;
                        case "PLAYERTURN":
                            if (ShowNextUnitTurn.ReturnAnimFinished())
                            {
                                if (setedPlayerTurn == false)
                                {
                                    _playerTurn();
                                }
                            }
                            else
                            {
                                ShowNextUnitTurn.StartNextTurnAnim();
                            }
                            break;
                        case "COMP1":
                            if (ShowNextUnitTurn.ReturnAnimFinished())
                            {
                                if (setedComp1Turn == false)
                                {
                                    comp1Turn();
                                }
                            }
                            else
                            {
                                ShowNextUnitTurn.StartNextTurnAnim();
                            }
                            break;
                        case "COMP2":
                            if (ShowNextUnitTurn.ReturnAnimFinished())
                            {
                                if (setedComp2Turn == false)
                                {
                                    comp2Turn();
                                }
                            }
                            else
                            {
                                ShowNextUnitTurn.StartNextTurnAnim();
                            }
                            break;
                        case "ENEMYTURN":
                            if (ShowNextUnitTurn.ReturnAnimFinished())
                            {
                                _enemyTurn();
                            }
                            else
                            {
                                ShowNextUnitTurn.StartNextTurnAnim();
                            }
                            break;
                        case "EOR":
                            EndOfRound();
                            break;
                        case "SETTURNS":
                            SetTurns();
                            break;
                    }
                }
            }
            else if (moveCam)
            {
                if (CamIsOnPos(cameraTravelPoints[pointsIndex]) == false)
                {
                    target = new Vector3(cameraTravelPoints[pointsIndex].x, cameraTravelPoints[pointsIndex].y,
                        mainCamera.transform.position.z);
                    mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, target, 0.0005f);
                }
                else
                {
                    if (pointsIndex < cameraTravelPoints.Length - 1)
                    {
                        pointsIndex++;
                    }
                    else
                    {
                        pointsIndex = 0;
                    }
                }
            }
        }
    }

    private void ResetCommandCanvas()
    {
        var allUnits = FindObjectsOfType<Unit>();

        for (int x = 0; x < allUnits.Length; x++)
        {
            var actionOwnerName = acctionC.ReturnOwner().GetComponent<Unit>().unitName;

            if (allUnits[x].GetComponent<CheckAnimationState>())
            {
                unitAnimationState = allUnits[x].GetComponent<CheckAnimationState>();
            }
            
            if (allUnits[x].name != "endofround" && allUnits[x].unitName == actionOwnerName &&
                unitAnimationState.animationWasFinished)
            {
                if (_attackTarget != null && _attackTarget.GetComponent<CheckAnimationState>().animationWasFinished)
                {
                    allUnits[x].resetAllAnims();
                    _attackTarget.GetComponent<Unit>().resetAllAnims();
                    allUnits[x].GetComponent<battleWalk>().ActivateCommandsMenu();
                    canvasResetSuccessfully = true;
                    enemyWasAttacked = false;
                    acctionC.Resetar();
                    break;
                }
            }
        }
    }

    private void DisableAllCommandsMenu()
    {
        var allUnits = FindObjectsOfType<Unit>();

        for (int x = 0; x < allUnits.Length; x++)
        {
            if (allUnits[x].name != "endofround" && allUnits[x].gameObject.GetComponent<battleWalk>())
            {
                allUnits[x].GetComponent<battleWalk>().DeactivateCommandsMenu();
            }
        }
    }

    public bool ReturnOutOfMenu()
    {
        return outOfMenu;
    }

    public bool ReturnInitialFade()
    {
        return initialFade;
    }

    public void DeactivateMenu()
    {
        if (!moveCam)
        {
            moveCam = false;
        }
    }
    
    public void StartGame()
    {
        if (!outOfMenu)
        {
            turnHud.SetActive(true);
            battleStatus.SetActive(true);
            outOfMenu = true;
        }
    }

    public void ActivateMenu()
    {
        initialFade = true;
        var allMenuButtons = menuPanel.transform.GetComponentsInChildren<Button>();
        for (int x = 0; x < allMenuButtons.Length; x++)
        {
            allMenuButtons[x].interactable = true;
        }
        Debug.Log("ACTIVATE MENU!");
    }

    public void QuitGame()
    {
        Debug.Log("GAME OUT!");
        Application.Quit();
    }

    private bool CamIsOnPos(Vector3 posToGo)
    {
        var mainCamPos = mainCamera.transform.position;

        if (mainCamPos.x == posToGo.x && mainCamPos.y == posToGo.y)
        {
            return true;
        }

        return false;
    }

    public void changeStateToStart()
    {
        state = BattleState.START;
    }

    void comp2Turn()
    {
        if (companion2Prefab.GetComponent<Unit>().isDead == false) 
        { 
            setedComp2Turn = true;
            battleStatusText.text = "Companion 2 Turn";
            companion2Prefab.GetComponent<battleWalk>().ActivateCommandsCanvas();
        } 
        else 
        { 
            SkipTurn(2); 
        } 
    }

    public void OnComp2AttackButton(GameObject enemyAttacked)
    {
        if (state != BattleState.COMP2)
        {
            return;

        }
        else
        {
            _attackTarget = enemyAttacked;
            canvasResetSuccessfully = false;
            acctionC.SetOwner(companion2Prefab.GetComponent<battleWalk>());
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

        if (!comp2DealtDamage)
        {
            BattleRating.DamageDealt = BattleRating.DamageDealt - companion2Prefab.GetComponent<Unit>().damage;
            comp2DealtDamage = true;
        }
        
        yield return new WaitForSeconds(.5f);

        if (isDead)
        {
            enemyPrefab.GetComponent<Unit>().playSound(3);
            state = BattleState.WON;
            EndBattle();
        }
    }
    void comp1Turn()
    {
        if (companion1Prefab.GetComponent<Unit>().isDead == false) 
        { 
            setedComp1Turn = true;
            battleStatusText.text = "Companion 1 Turn";
            companion1Prefab.GetComponent<battleWalk>().ActivateCommandsCanvas();
        } 
        else 
        { 
            SkipTurn(1); 
        } 
    }

    public void OnComp1AttackButton (GameObject enemyAttacked)
    {
        if(state != BattleState.COMP1)
        {
            return;
        }
        else
        {
            _attackTarget = enemyAttacked;
            canvasResetSuccessfully = false;
            acctionC.SetOwner(companion1Prefab.GetComponent<battleWalk>());
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
        }
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

        if (!comp1DealtDamage)
        {
            BattleRating.DamageDealt = BattleRating.DamageDealt - companion1Prefab.GetComponent<Unit>().damage;
            comp1DealtDamage = true;
        }
        
        yield return new WaitForSeconds(.1f);

        if (isDead)
        {
            enemyPrefab.GetComponent<Unit>().playSound(3);
            state = BattleState.WON;
            EndBattle();
        }
    }
    void _playerTurn()
    {        
        setedPlayerTurn = true;
        playerPrefab.GetComponent<battleWalk>().ActivateCommandsCanvas();
        battleStatusText.text = "Your Turn";       
    }
    
    IEnumerator checkAttack(GameObject enemyAttacked)
    {
        yield return new WaitForSeconds(2.7f);
        
        if (successfullAttack & state==BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack(enemyAttacked));
        }
        else if(successfullAttack & state == BattleState.COMP1)
        {
            StartCoroutine(comp1Attack(enemyAttacked));
        }
        else if(successfullAttack & state == BattleState.COMP2)
        {
            StartCoroutine(comp2Attack(enemyAttacked));
        }
        else if (successfullAttack == false)
        {
            var actionOwner = acctionC.ReturnOwner().GetComponent<Unit>();
            actionOwner.resetAllAnims();
            _attackTarget.GetComponent<Unit>().resetAllAnims();
            actionOwner.GetComponent<battleWalk>().ActivateCommandsMenu();
            canvasResetSuccessfully = true;
            enemyWasAttacked = false;
            acctionC.Resetar();
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

        if (!playerDealtDamage)
        {
            BattleRating.DamageDealt = BattleRating.DamageDealt - playerPrefab.GetComponent<Unit>().damage;
            playerDealtDamage = true;
        }
        
        yield return new WaitForSeconds(.5f);

        if (isDead == true)
        {
            enemyPrefab.GetComponent<Unit>().playSound(3);
            state = BattleState.WON;
            EndBattle();
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
                enemyPrefab.GetComponent<TimerForTurn>().Iniciar(1.5f);
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

        if (CheckForAllUnitsAnimation() && enemyPrefab.GetComponent<EnemyBattleWalk>().ReturnEndTurn() && setedEnemyTurn && timerForEnemyTurn)      
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
            enemyWasAttacked = true;
        }
        else if(state == BattleState.COMP2)
        {
            OnComp2AttackButton(enemyAttacked);
            enemyWasAttacked = true;
        }
        else
        {
            _attackTarget = enemyAttacked;
            canvasResetSuccessfully = false;
            acctionC.SetOwner(playerPrefab.GetComponent<battleWalk>());
            acctionC.Ativar();
            StartCoroutine(checkAttack(enemyAttacked));
            enemyWasAttacked = true;
        }
    }

    void EndBattle()
    {
        DisableAllCommandsMenu();
        
        playerPrefab.GetComponent<battleWalk>().ChangeMoveBool(false);
        companion1Prefab.GetComponent<battleWalk>().ChangeMoveBool(false);
        companion2Prefab.GetComponent<battleWalk>().ChangeMoveBool(false);
        enemyPrefab.GetComponent<EnemyBattleWalk>().ChangeCanAct(false);
        
        if (state == BattleState.WON)
        {
            BattleRating.Companion1Alive = !companion1Prefab.GetComponent<Unit>().isDead;
            BattleRating.Companion2Alive = !companion2Prefab.GetComponent<Unit>().isDead;
            battleStatus.SetActive(false);
            turnHud.SetActive(false);
            fadeImageCode.LoadEndScreen();
            battleStatusText.text = "You Win";
            won = true;
        }
        else if (state == BattleState.LOST)
        {
            battleStatusText.text = "You Lose";
            battleStatus.SetActive(false);
            turnHud.SetActive(false);
            Debug.Log("LoadingGameOver!!!");
            fadeImageCode.LoadGameOver();
            lost = true;
        }
    }

    public bool ReturnWon()
    {
        return won;
    }

    public bool ReturnLost()
    {
        return lost;
    }
    
    public void EndOfRound()
    {
        for (int i = 0; i < chars.Count; i++)
        {
            chars[i].GetComponent<Unit>().unitHasPlayed = false;
            chars[i].GetComponent<Unit>().currentTurn += 1;
            chars[i].GetComponent<Unit>().CheckStatus();
            if (chars[i].GetComponent<Unit>().unitName == "Companion 1")
            {
                chars[i].GetComponent<Unit>().SetWaspPassive();
                chars[i].GetComponent<battleWalk>().DecreaseSkillsCD();
            }
            else if(chars[i].GetComponent<Unit>().unitName == "Companion 2")
            {
                chars[i].GetComponent<Unit>().SetFrogPassive();
                chars[i].GetComponent<battleWalk>().DecreaseSkillsCD();
            }
            else if(chars[i].GetComponent<Unit>().unitName == "Player")
            {
                chars[i].GetComponent<battleWalk>().DecreaseSkillsCD();
            }
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
        needsToSkipTurn = true;
        entityToSkipTurn = entity;
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
    
    public bool CheckForAllUnitsAnimation()
    {
        var cont = 0;
        
        var allUnits = FindObjectsOfType<Unit>();
        
        for (int x = 0; x < allUnits.Length; x++)
        {
            if (allUnits[x].name != "endofround" && allUnits[x].GetComponent<CheckAnimationState>())
            {
                var animationOver = allUnits[x].GetComponent<CheckAnimationState>().animationWasFinished;
                
                if (!animationOver && !allUnits[x].ReturnAllAnimsBolls())
                {
                    ////TESTING BRO
                    Debug.Log("Waiting for animation to finish!");
                }
                else if(animationOver && !allUnits[x].ReturnAllAnimsBolls())
                {
                    allUnits[x].resetAllAnims();
                }
                else
                {
                    cont++;
                }
            }
        }

        if (cont >= 9)
        {
            return true;
        }

        return false;
    }

    private void EndingTurn()
    {
        switch (entityToEndTurn)
        {
            case 0:
                playerPrefab.GetComponent<Unit>().unitHasPlayed = true;
                playerHasPlayed = true;
                endTurn = true;
                playerPrefab.GetComponent<battleWalk>().Commandos.SetActive(false);
                playerPrefab.GetComponent<battleWalk>().Commandos.transform.GetChild(0).gameObject.SetActive(true);
                playerPrefab.GetComponent<battleWalk>().SetMyTurnToFalse();
                ShowNextUnitTurn.ResetReturnAnim();
                break;
            case 1:
                companion1Prefab.GetComponent<Unit>().unitHasPlayed = true;
                Comp1HasPlayed = true;
                endTurn = true;
                companion1Prefab.GetComponent<battleWalk>().SetMyTurnToFalse();
                ShowNextUnitTurn.ResetReturnAnim();
                break;
            case 2:
                companion2Prefab.GetComponent<Unit>().unitHasPlayed = true;
                Comp2HasPlayed = true;
                endTurn = true;
                companion2Prefab.GetComponent<battleWalk>().SetMyTurnToFalse();
                ShowNextUnitTurn.ResetReturnAnim();
                break;
        }
        needsToEndTurn = false;
    }

    private void SkippingTurn()
    {
        switch (entityToSkipTurn)
        {
            case 0:
                playerHasPlayed = true;
                playerPrefab.GetComponent<Unit>().unitHasPlayed = true;
                endTurn = true;
                playerPrefab.GetComponent<battleWalk>().SetMyTurnToFalse();
                ShowNextUnitTurn.ResetReturnAnim();
                break;
            case 1:
                Comp1HasPlayed = true;
                companion1Prefab.GetComponent<Unit>().unitHasPlayed = true;
                endTurn = true;
                companion1Prefab.GetComponent<battleWalk>().SetMyTurnToFalse();
                ShowNextUnitTurn.ResetReturnAnim();
                break;
            case 2:
                Comp2HasPlayed = true;
                companion2Prefab.GetComponent<Unit>().unitHasPlayed = true;
                endTurn = true;
                companion2Prefab.GetComponent<battleWalk>().SetMyTurnToFalse();
                ShowNextUnitTurn.ResetReturnAnim();
                break;
            case 3:
                enemyHasPlayed = true;
                enemyPrefab.GetComponent<Unit>().unitHasPlayed = true;
                endTurn = true;
                ShowNextUnitTurn.ResetReturnAnim();
                break;
        }
        
        SetTurns();

        needsToSkipTurn = false;
    }
    
    public void EndOfTurn(int entity)
    {
        Debug.Log("Turn end");
        needsToEndTurn = true;
        entityToEndTurn = entity;
    }
}
