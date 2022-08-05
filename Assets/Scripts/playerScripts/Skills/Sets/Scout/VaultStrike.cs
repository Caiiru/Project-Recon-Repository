using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VaultStrike:Skill
{
    public Button SkillButton;

    public TextMeshProUGUI SkillCD;
    
    [SerializeField] LayerMask layermask;

    public LayerMask enemymask, LayerMaskToIgnore, LayerMaskToIgnore2;

    [SerializeField] bool usingSkill;
    
    private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;

    private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    private string sideToSend;

    private bool canUseSkill;
    
    public void Attack()
    {
        baixo.SetActive(true);
        cima.SetActive(true);
        esquerda.SetActive(true);
        direita.SetActive(true);
        usingSkill = true;
    }

    private void Start()
    {
        baixo = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        cima = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject;
        esquerda = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject;
        direita = this.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.GetChild(3).gameObject;

        animbaixo = baixo.GetComponent<Animator>();
        animcima = cima.GetComponent<Animator>();
        animesquerda = esquerda.GetComponent<Animator>();
        animdireita = direita.GetComponent<Animator>();
    }
    private void Update()
    {
        canUseSkill = ReturnCanUseSkill();
        
        SkillCD.text = ReturnCDNumber().ToString();

        if (canUseSkill)
        {
            SkillCD.text = " ";
            SkillButton.interactable = true;
        }
            
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (usingSkill && canUseSkill)
        {
            if (Input.GetButtonDown("Fire2") && gameObject.GetComponent<battleWalk>().ReturnMyTurn())
            {
                gameObject.GetComponent<battleWalk>().setSkillCommandCanvas(true);
                hideRange();
            }

            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, layermask);

            if (raycast.collider != null)
            {
                if (raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        sideToSend = raycast.collider.name;
                        checkContact();
                        SetCooldown();
                        gameObject.GetComponent<Unit>().playSound(1);
                    }
                }
            }
            else
            {
                animbaixo.SetBool("slashOver", false);
                animcima.SetBool("slashOver", false);
                animdireita.SetBool("slashOver", false);
                animesquerda.SetBool("slashOver", false);
            }
        }
    }
    void checkContact()
    {
       var hitEnemy = false;
       var EnemyGameObject = GameObject.Find("Enemy3x3 (1)");
       var EnemyPartsUnit = EnemyGameObject.GetComponent<EnemyParts>().ReturnAllEnemyParts();

        for (int x = 0; x < EnemyPartsUnit.Length - 1; x++)
        {
            var Vector2Pos = new Vector2(EnemyPartsUnit[x].gameObject.transform.position.x, EnemyPartsUnit[x].gameObject.transform.position.y);
            RaycastHit2D hit2d = new RaycastHit2D();
            hit2d = Physics2D.Raycast(Vector2Pos, Vector3.back, Mathf.Infinity, layermask);
            Debug.DrawRay(Vector2Pos, Vector3.back, Color.magenta, Mathf.Infinity);

            if (hit2d && hit2d.collider)
            {
                if (hit2d.collider.CompareTag("Skill") && hit2d.collider.name == sideToSend)
                {
                    if (EnemyPartsUnit[x].transform.parent != null)
                    {
                        if (EnemyPartsUnit[x].currentHP <= 0)
                        {
                            EnemyGameObject.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                            Debug.Log("HIT ENEMY!");
                            hitEnemy = true;
                        }
                        else
                        {
                            EnemyPartsUnit[x].TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                            Debug.Log("HIT ENEMY PART(" + x + ") AKA: " + EnemyPartsUnit[x].name);
                            hitEnemy = true;
                        }
                    }
                    else
                    {
                        EnemyGameObject.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                        Debug.Log("HIT ENEMY!");
                        hitEnemy = true;
                    }
                }
            }
        }

        for (int x = 0; x < 5; x++)
        {
            var Vector2PosEnemy = new Vector2(EnemyGameObject.gameObject.transform.position.x,
                EnemyGameObject.gameObject.transform.position.y);
            switch (x)
            {
                case 1:
                    //RIGHT
                    Vector2PosEnemy.x = Vector2PosEnemy.x + 0.5f;
                    Vector2PosEnemy.y = Vector2PosEnemy.y - 0.25f;
                    break;
                case 2:
                    //UP
                    Vector2PosEnemy.x = Vector2PosEnemy.x + 0.5f;
                    Vector2PosEnemy.y = Vector2PosEnemy.y + 0.25f;
                    break;
                case 3:
                    //LEFT
                    Vector2PosEnemy.x = Vector2PosEnemy.x - 0.5f;
                    Vector2PosEnemy.y = Vector2PosEnemy.y + 0.25f;
                    break;
                case 4:
                    //DOWN
                    Vector2PosEnemy.x = Vector2PosEnemy.x - 0.5f;
                    Vector2PosEnemy.y = Vector2PosEnemy.y - 0.25f;
                    break;
            }

            RaycastHit2D hit2D = new RaycastHit2D();
            hit2D = Physics2D.Raycast(Vector2PosEnemy, Vector3.back, Mathf.Infinity, layermask);
            Debug.DrawRay(Vector2PosEnemy, Vector3.back, Color.red, Mathf.Infinity);

            if (hit2D && hit2D.collider)
            {
                if (hit2D.collider.CompareTag("Skill") && hit2D.collider.name == sideToSend)
                {
                    EnemyGameObject.GetComponent<Unit>()
                        .TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                    Debug.Log("HIT ENEMY!");
                    hitEnemy = true;
                }
            }
        }

        if (hitEnemy)
        {
            Debug.Log("HIT ENEMY, TRYING TO MOVE PLAYER!");
            MovePlayer();
        }
        
        hideRange();
        GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(0);
    }

    private void MovePlayer()
    {
        var breakLoop = false;
        var Vector2PlayerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        var Vector2MovementPos = Vector2PlayerPos;      
        
        Debug.Log("SIDE TO SEND: " + sideToSend);
        
        for (int x = 0; x < 6; x++)
        {
            Debug.Log("PLAYER POS: " + Vector2PlayerPos);
            Debug.Log("MOVEMENT POS: " + Vector2MovementPos);

            if (breakLoop)
            {
                break;
            }

            switch (sideToSend)
            {
                case "direita":
                    var hit2D = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        ~LayerMaskToIgnore & ~layermask & ~LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.green, Mathf.Infinity);

                    var hit2DArena = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.yellow, Mathf.Infinity);

                    Debug.DrawRay(Vector2MovementPos, Vector3.up, Color.red, Mathf.Infinity);

                    if (hit2D)
                    {
                        Debug.Log("HIT2D NAME: " + hit2D.transform.name);
                        Debug.Log("HIT2D TAG: " + hit2D.transform.tag);
                        Vector2MovementPos = new Vector2(Vector2MovementPos.x + 0.5f, Vector2MovementPos.y - 0.25f);
                    }
                    else
                    {
                        if (hit2DArena)
                        {
                            gameObject.transform.position = new Vector3(Vector2MovementPos.x, Vector2MovementPos.y,
                                gameObject.transform.position.z);
                            breakLoop = true;
                        }
                    }
                break;
                case "cima":
                    hit2D = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        ~LayerMaskToIgnore & ~layermask & ~LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.green, Mathf.Infinity);

                    hit2DArena = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.yellow, Mathf.Infinity);

                    Debug.DrawRay(Vector2MovementPos, Vector3.up, Color.red, Mathf.Infinity);

                    if (hit2D)
                    {
                        Debug.Log("HIT2D NAME: " + hit2D.transform.name);
                        Debug.Log("HIT2D TAG: " + hit2D.transform.tag);
                        Vector2MovementPos = new Vector2(Vector2MovementPos.x + 0.5f, Vector2MovementPos.y + 0.25f);
                    }
                    else
                    {
                        if (hit2DArena)
                        {
                            gameObject.transform.position = new Vector3(Vector2MovementPos.x, Vector2MovementPos.y,
                                gameObject.transform.position.z);
                            breakLoop = true;
                        }
                    }
                break;
                case "esquerda":
                    hit2D = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        ~LayerMaskToIgnore & ~layermask & ~LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.green, Mathf.Infinity);

                    hit2DArena = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.yellow, Mathf.Infinity);

                    Debug.DrawRay(Vector2MovementPos, Vector3.up, Color.red, Mathf.Infinity);

                    if (hit2D)
                    {
                        Debug.Log("HIT2D NAME: " + hit2D.transform.name);
                        Debug.Log("HIT2D TAG: " + hit2D.transform.tag);
                        Vector2MovementPos = new Vector2(Vector2MovementPos.x - 0.5f, Vector2MovementPos.y + 0.25f);
                    }
                    else
                    {
                        if (hit2DArena)
                        {
                            gameObject.transform.position = new Vector3(Vector2MovementPos.x, Vector2MovementPos.y,
                                gameObject.transform.position.z);
                            breakLoop = true;
                        }
                    }
                break;
                case "baixo":
                    hit2D = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        ~LayerMaskToIgnore & ~layermask & ~LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.green, Mathf.Infinity);

                    hit2DArena = Physics2D.Raycast(Vector2MovementPos, Vector3.back, Mathf.Infinity,
                        LayerMaskToIgnore2);
                    Debug.DrawRay(Vector2MovementPos, Vector3.back, Color.yellow, Mathf.Infinity);

                    Debug.DrawRay(Vector2MovementPos, Vector3.up, Color.red, Mathf.Infinity);

                    if (hit2D)
                    {
                        Debug.Log("HIT2D NAME: " + hit2D.transform.name);
                        Debug.Log("HIT2D TAG: " + hit2D.transform.tag);
                        Vector2MovementPos = new Vector2(Vector2MovementPos.x - 0.5f, Vector2MovementPos.y - 0.25f);
                    }
                    else
                    {
                        if (hit2DArena)
                        {
                            gameObject.transform.position = new Vector3(Vector2MovementPos.x, Vector2MovementPos.y,
                                gameObject.transform.position.z);
                            breakLoop = true;
                        }
                    }
                break;
            }
        }
    }

    private void SetCooldown()
    {
        SetCD();
        SkillButton.interactable = false;
        SkillCD.text = ReturnCDNumber().ToString();
        SkillUsedThisTurn = true;
    }
    
    private void hideRange()
    {
        baixo.SetActive(false);
        cima.SetActive(false);
        esquerda.SetActive(false);
        direita.SetActive(false);
        usingSkill = false;
        animbaixo.SetBool("isOver", false);
        animesquerda.SetBool("isOver", false);
        animcima.SetBool("isOver", false);
        animdireita.SetBool("isOver", false);
    }
}


