using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slash : Skill
{    
    public Button SkillButton;

    public TextMeshProUGUI SkillCD;
    
    public GameObject SlashGO;
    private GameObject slash1;
    private GameObject slash2;
    private GameObject slash3;
    private GameObject slash4;
    private Animator animator1;
    private Animator animator2;
    private Animator animator3;
    private Animator animator4;
    public LayerMask skillMask;
    public bool isAttacking = false;
    
    private string sideToSend;

    private bool canUseSkill;
    
    void Start()
    {
        //Instanciar os objetos e os filhos (Colliders e Marcadores)
        slash1 = SlashGO.transform.GetChild(0).gameObject; //baixo
        slash2 = SlashGO.transform.GetChild(1).gameObject; //esquerda
        slash3 = SlashGO.transform.GetChild(2).gameObject; //cima
        slash4 = SlashGO.transform.GetChild(3).gameObject; //direita
        
        animator1 = slash1.GetComponent<Animator>();
        animator2 = slash2.GetComponent<Animator>();
        animator3 = slash3.GetComponent<Animator>();
        animator4 = slash4.GetComponent<Animator>();
    }

    void Update()
    {
        canUseSkill = ReturnCanUseSkill();
        
        SkillCD.text = ReturnCDNumber().ToString();

        if (canUseSkill)
        {
            SkillCD.text = " ";
            SkillButton.interactable = true;
        }
        
        if (isAttacking && canUseSkill)
        {
            if (Input.GetButtonDown("Fire2") && gameObject.GetComponent<battleWalk>().ReturnMyTurn())
            {
                hideRange(); 
                gameObject.GetComponent<battleWalk>().setSkillCommandCanvas(true);
            }
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, skillMask);
           
            if(hit2d && hit2d.collider)
            {
                if(hit2d.collider.CompareTag("Skill") && hit2d.collider.GetComponent<onMouseOver>())
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        EnableTrueCollider();
                        sideToSend = hit2d.collider.name;
                        checkContact();
                        SetCooldown();
                        gameObject.GetComponent<Unit>().playSound(1);
                    }
                }
            }
        }
    }

    private void EnableTrueCollider()
    {
        slash1.GetComponent<onMouseOver>().EnableTrueCollider();
        slash2.GetComponent<onMouseOver>().EnableTrueCollider();
        slash3.GetComponent<onMouseOver>().EnableTrueCollider();
        slash4.GetComponent<onMouseOver>().EnableTrueCollider();
    }

    public void Attack() { //show range
        //Skill ativa
        slash1.SetActive(true);
        slash2.SetActive(true);
        slash3.SetActive(true);
        slash4.SetActive(true);
        isAttacking = true;
    }


    void checkContact()
    {
        Debug.Log("SIDE CHOOSEN: " + sideToSend);
        
        var EnemyGameObject = GameObject.Find("Enemy3x3 (1)");
        var EnemyPartsUnit = EnemyGameObject.GetComponent<EnemyParts>().ReturnAllEnemyParts();
        
        for (int x = 0; x < EnemyPartsUnit.Length - 1; x++)
        {
            var Vector2Pos = new Vector2(EnemyPartsUnit[x].gameObject.transform.position.x, EnemyPartsUnit[x].gameObject.transform.position.y);
            RaycastHit2D hit2d = new RaycastHit2D();
            hit2d = Physics2D.Raycast(Vector2Pos, Vector3.back, Mathf.Infinity, skillMask);
            Debug.DrawRay(Vector2Pos, Vector3.back, Color.magenta, Mathf.Infinity);

            if (hit2d && hit2d.collider)
            {
                if (hit2d.collider.CompareTag("Skill") && hit2d.collider.name == sideToSend)
                {
                    if (EnemyPartsUnit[x].transform.parent != null)
                    {
                        if (EnemyPartsUnit[x].currentHP <= 0)
                        {
                            EnemyGameObject.GetComponent<Unit>()
                                .TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                            Debug.Log("HIT ENEMY!");
                            Debug.DrawRay(hit2d.point, Vector2.left, Color.blue, Mathf.Infinity);
                        }
                        else
                        {
                            EnemyPartsUnit[x].TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                            Debug.Log("HIT ENEMY PART(" + x + ") AKA: " + EnemyPartsUnit[x].name);
                            Debug.DrawRay(hit2d.point, Vector2.left, Color.blue, Mathf.Infinity);
                        }
                    }
                    else
                    {
                        EnemyGameObject.GetComponent<Unit>()
                            .TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                        Debug.Log("HIT ENEMY!");
                        Debug.DrawRay(hit2d.point, Vector2.left, Color.blue, Mathf.Infinity);
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
            hit2D = Physics2D.Raycast(Vector2PosEnemy, Vector3.back, Mathf.Infinity, skillMask);
            Debug.DrawRay(Vector2PosEnemy, Vector3.back, Color.red, Mathf.Infinity);

            if (hit2D && hit2D.collider)
            {
                Debug.Log("HIT SOMETHING");
                Debug.Log("HIT2D TAG: " + hit2D.collider.tag);
                Debug.Log("HIT2D NAME: "+ hit2D.collider.name);
                Debug.Log("SIDE TO SEND: " + sideToSend);
                
                if (hit2D.collider.CompareTag("Skill") && hit2D.collider.name == sideToSend)
                {
                    EnemyGameObject.GetComponent<Unit>()
                        .TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                    Debug.Log("HIT ENEMY!");
                    Debug.DrawRay(hit2D.point, Vector2.left, Color.blue, Mathf.Infinity);
                }
            }
        }
        
        hideRange();
        GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(0);
    }
    
    private void SetCooldown()
    {
        SetCD();
        SkillButton.interactable = false;
        SkillCD.text = ReturnCDNumber().ToString();
        SkillUsedThisTurn = true;
    }
    
    void hideRange()
    {       
        slash1.GetComponent<onMouseOver>().EnableFakeCollider();
        slash2.GetComponent<onMouseOver>().EnableFakeCollider();
        slash3.GetComponent<onMouseOver>().EnableFakeCollider();
        slash4.GetComponent<onMouseOver>().EnableFakeCollider();
        slash1.SetActive(false);
        slash2.SetActive(false);
        slash3.SetActive(false);
        slash4.SetActive(false);
        isAttacking = false;
        animator1.SetBool("slashOver", false);
        animator2.SetBool("slashOver", false);
        animator3.SetBool("slashOver", false);
        animator4.SetBool("slashOver", false);
    }
}
