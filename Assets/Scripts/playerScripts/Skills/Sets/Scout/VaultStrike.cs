using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultStrike:Skill
{

    private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;

    private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    [SerializeField] LayerMask layermask;

    public LayerMask enemymask;

    [SerializeField] bool usingSkill = false;
    public VaultStrike(){


    }


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
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (usingSkill)
        {
            if (Input.GetButtonDown("Fire2"))
            {
               
                this.GetComponent<battleWalk>().setSkillCommandCanvas(true);
                hideRange();
            }

            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, layermask);

            if (raycast.collider.CompareTag("Skill"))
            {
                raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                if (Input.GetButtonDown("Fire1"))
                {
                    checkContact(raycast.collider.name,raycast);
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
    void checkContact(string name, RaycastHit2D raycast)
    {
        GameObject cl = GameObject.Find(name);
        RaycastHit2D line = Physics2D.Linecast(transform.position, cl.transform.GetChild(0).transform.position, enemymask);
        Debug.DrawLine(gameObject.transform.position, cl.transform.GetChild(0).transform.position, Color.blue);
        if (line.collider.CompareTag("EnemyPart"))
        {
            if (line.collider.GetComponent<Unit>().currentHP <= 0)
            {
                var en = line.collider.gameObject.transform.parent.gameObject;

                en.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
            }
            else
            {
                line.collider.gameObject.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
            }

            hideRange();
            GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(0);
            this.gameObject.GetComponent<battleWalk>().vaultStrikeMove(raycast.collider.transform.position);
        }


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


