using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Skill
{
    [SerializeField] private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;

    [SerializeField] private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    public bool usingSkill = false;


    [SerializeField] LayerMask layermask;
    [SerializeField] LayerMask enemyMask;

    [SerializeField] Vector3 trys;


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
        baixo = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        cima = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        esquerda = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject;
        direita = this.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject;


        animbaixo = baixo.GetComponent<Animator>();
        animcima = cima.GetComponent<Animator>();
        animesquerda = esquerda.GetComponent<Animator>();
        animdireita = direita.GetComponent<Animator>();



    }

    private void Update()
    {
        if (usingSkill)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newMousePosition = new Vector3(worldMousePosition.x, worldMousePosition.y, 0);
            trys = newMousePosition;

            if (Input.GetButtonDown("Fire2"))
            {
                hideRange();
                this.GetComponent<battleWalk>().setSkillCommandCanvas(true);
            }
            RaycastHit2D raycast = Physics2D.Raycast(newMousePosition, Vector3.forward, Mathf.Infinity, layermask);

            if (raycast.collider != null)
            {

                if (raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        checkContact(raycast.collider.name);
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
        void hideRange()
        {
            baixo.SetActive(false);
            cima.SetActive(false);
            esquerda.SetActive(false);
            direita.SetActive(false);
            usingSkill = false;
            animbaixo.SetBool("slashOver", false);
            animesquerda.SetBool("slashOver", false);
            animcima.SetBool("slashOver", false);
            animdireita.SetBool("slashOver", false);
        }
        void checkContact(string name)
        {

            GameObject cl = GameObject.Find(name);
            RaycastHit2D line = Physics2D.Linecast(transform.position, cl.transform.GetChild(0).transform.position, enemyMask);
            Debug.DrawLine(gameObject.transform.position, cl.transform.GetChild(0).transform.position, Color.blue);

       
        /*
        if (line.collider != null)
            {
                if (line.collider.GetComponent<Unit>().currentHP <= 0)
                {
                    var en = line.collider.gameObject.transform.parent.gameObject;

                    en.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                }
                else
                {
                line.collider.gameObject.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
                skillEffect(line.collider.gameObject);    
            }

            }
            else
            {
                Debug.Log("pass");
            }
        */

            hideRange();
            GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(1);
        }

    void skillEffect(GameObject MarkTarget)
    {
        MarkTarget.GetComponent<Unit>().AddStatusEffect(6);
    }
}
