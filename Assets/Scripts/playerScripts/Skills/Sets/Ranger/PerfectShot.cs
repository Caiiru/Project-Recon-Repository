using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectShot : Skill
{

    public PerfectShot(){

    }
    private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;

    private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    [SerializeField] LayerMask layermask;

    [SerializeField] bool usingSkill = false;



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
        baixo = this.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject;
        cima = this.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject;
        esquerda = this.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject;
        direita = this.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject.transform.GetChild(3).gameObject;

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
                baixo.SetActive(false);
                cima.SetActive(false);
                esquerda.SetActive(false);
                direita.SetActive(false);
                this.GetComponent<battleWalk>().setSkillCommandCanvas(true);
                usingSkill = false;
                animbaixo.SetBool("isOver", false);
                animesquerda.SetBool("isOver", false);
                animcima.SetBool("isOver", false);
                animdireita.SetBool("isOver", false);
            }

            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, layermask);

            if (raycast.collider.CompareTag("Skill"))
            {
                raycast.collider.gameObject.GetComponent<Animator>().SetBool("isOver", true);
            }
            else
            {
                animbaixo.SetBool("isOver", false);
                animcima.SetBool("isOver", false);
                animdireita.SetBool("isOver", false);
                animesquerda.SetBool("isOver", false);

            }

        }
    }
}
