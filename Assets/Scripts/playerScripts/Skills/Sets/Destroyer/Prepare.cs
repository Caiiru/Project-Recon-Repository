using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prepare : Skill
{
    public Prepare()
    {

    }

    private GameObject allsides;
    private Animator animall;

    [SerializeField] LayerMask layerMask;
    bool usingSkill = false;
    public void Attack()
    {
        allsides.SetActive(true);
        usingSkill = true;

    }

    private void Start()
    {
        allsides = this.transform.GetChild(1).gameObject.transform.GetChild(10).gameObject.transform.GetChild(0).gameObject;

        animall = allsides.GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (usingSkill)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                allsides.SetActive(false);
                this.GetComponent<battleWalk>().setSkillCommandCanvas(true);
                usingSkill = false;
                animall.SetBool("isOver", false);
            }

            RaycastHit2D ray = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, layerMask);
            if (ray.collider.CompareTag("Skill"))
            {
                ray.collider.gameObject.GetComponent<Animator>().SetBool("isOver", true);

            }
            else
            {
                animall.SetBool("isOver", false);
            }
        }
    }
}
