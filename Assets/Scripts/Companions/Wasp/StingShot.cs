using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingShot : Skill
{
    private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;


    private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    public LayerMask layermask;

    private string sideToSend;

    [SerializeField] bool usingSkill = false;
    void Start()
    {
        baixo = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        esquerda = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        cima = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
        direita = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).gameObject;

        animbaixo = baixo.GetComponent<Animator>();
        animcima = cima.GetComponent<Animator>();
        animesquerda = esquerda.GetComponent<Animator>();
        animdireita = direita.GetComponent<Animator>();
    }
    public void Attack()
    {
        baixo.SetActive(true);
        cima.SetActive(true);
        esquerda.SetActive(true);
        direita.SetActive(true);
        usingSkill = true;
    }
    void Update()
    {
        if (usingSkill)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetButtonDown("Fire2"))
            {

                this.GetComponent<battleWalk>().setSkillCommandCanvas(true);
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
                        string checkName = raycast.collider.name;
                        sideToSend = checkName;
                        checkContact();
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
                    if(EnemyPartsUnit[x].transform.parent != null)
                    {
                        EnemyGameObject.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                        Debug.Log("HIT ENEMY!");
                    }
                    else
                    {
                        EnemyPartsUnit[x].TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                        Debug.Log("HIT ENEMY PART("+x+") AKA: " +EnemyPartsUnit[x].name);
                    }
                }
                else
                {
                    EnemyGameObject.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                    Debug.Log("HIT ENEMY!");
                }
            }
        }

        for (int x = 0; x < 5; x++)
        {
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
            hit2D = Physics2D.Raycast(Vector2PosEnemy, Vector3.back, Mathf.Infinity, layermask);
            Debug.DrawRay(Vector2PosEnemy, Vector3.back, Color.red, Mathf.Infinity);

            if (hit2D && hit2D.collider)
            {
                if (hit2D.collider.CompareTag("Skill") && hit2D.collider.name == sideToSend)
                {
                    EnemyGameObject.GetComponent<Unit>()
                        .TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                    Debug.Log("HIT ENEMY!");
                    Debug.Log("ENEMY NAME: " + EnemyGameObject.name);
                    Debug.Log("ENEMY POS: " + EnemyGameObject.transform.position);
                    Debug.DrawRay(hit2D.point, Vector3.up, Color.green, Mathf.Infinity);
                }
            }
        }
        
        hideRange();
        GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(1);
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
}
