using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToxicDrain : Skill
{
    public Button SkillButton;

    public TextMeshProUGUI SkillCD;
    
    private bool canUseSkill;
    
    private bool usingSkill;
    
    [Header("Variables")]
    public int DamagePerStack = 4;
    public int CurePerStack = 5;

    private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;


    private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    private string sideToSend;
    
    public LayerMask layerMask;

    void Start()
    {
        baixo = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        esquerda = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        cima = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
        direita = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).gameObject;

        animbaixo = baixo.GetComponent<Animator>();
        animcima = cima.GetComponent<Animator>();
        animesquerda = esquerda.GetComponent<Animator>();
        animdireita = direita.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        canUseSkill = ReturnCanUseSkill();
        
        SkillCD.text = ReturnCDNumber().ToString();

        if (canUseSkill)
        {
            SkillCD.text = " ";
            SkillButton.interactable = true;
        }

        if (usingSkill && canUseSkill)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetButtonDown("Fire2") && gameObject.GetComponent<battleWalk>().ReturnMyTurn())
            {
                gameObject.GetComponent<battleWalk>().setSkillCommandCanvas(true);
                hideRange();
            }

            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, layerMask);

            if (raycast.collider != null)
            {
                if (raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        string checkName = raycast.collider.name;
                        sideToSend = checkName;
                        checkContact();
                        SetCooldown();
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
       var hasHit = false; 
       var EnemyGameObject = GameObject.Find("Enemy3x3 (1)");
       var EnemyPartsUnit = EnemyGameObject.GetComponent<EnemyParts>().ReturnAllEnemyParts();

        for (int x = 0; x < EnemyPartsUnit.Length - 1; x++)
        {
            if (hasHit)
            {
                break;
            }
            
            var Vector2Pos = new Vector2(EnemyPartsUnit[x].gameObject.transform.position.x, EnemyPartsUnit[x].gameObject.transform.position.y);
            RaycastHit2D hit2d = new RaycastHit2D();
            hit2d = Physics2D.Raycast(Vector2Pos, Vector3.back, Mathf.Infinity, layerMask);
            Debug.DrawRay(Vector2Pos, Vector3.back, Color.magenta, Mathf.Infinity);

            if (hit2d && hit2d.collider)
            {
                if (hit2d.collider.CompareTag("Skill") && hit2d.collider.name == sideToSend)
                {
                    var poisonNumber = EnemyGameObject.GetComponent<Unit>().checkPoison(EnemyGameObject);
                    EnemyGameObject.GetComponent<Unit>().TakeDamage(DamagePerStack * poisonNumber, elements.NEUTRO);
                    gameObject.GetComponent<Unit>().cureHP(poisonNumber * CurePerStack);
                    Debug.Log("HIT ENEMY!");
                    hasHit = true;
                }
            }
        }

        for (int x = 0; x < 5; x++)
        {
            if (hasHit)
            {
                break;
            }
            
            var Vector2PosEnemy = new Vector2(EnemyGameObject.gameObject.transform.position.x, EnemyGameObject.gameObject.transform.position.y);
            
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
            hit2D = Physics2D.Raycast(Vector2PosEnemy, Vector3.back, Mathf.Infinity, layerMask);
            Debug.DrawRay(Vector2PosEnemy, Vector3.back, Color.red, Mathf.Infinity);

            if (hit2D && hit2D.collider)
            {
                if (hit2D.collider.CompareTag("Skill") && hit2D.collider.name == sideToSend)
                {
                    var poisonNumber = EnemyGameObject.GetComponent<Unit>().checkPoison(EnemyGameObject);
                    Debug.Log("Poison number: " + poisonNumber);
                    EnemyGameObject.GetComponent<Unit>().TakeDamage(DamagePerStack * poisonNumber, elements.NEUTRO);
                    gameObject.GetComponent<Unit>().cureHP(poisonNumber * CurePerStack);
                    Debug.Log("HIT ENEMY!");
                    hasHit = true;
                }
            }
        }
        
        hideRange();
        GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(2);
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

    public void SetActiveSkill()
    {
        usingSkill = true;
        cima.SetActive(true);
        baixo.SetActive(true);
        esquerda.SetActive(true);
        direita.SetActive(true);
    }
}
