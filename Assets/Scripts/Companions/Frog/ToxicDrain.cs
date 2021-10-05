using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicDrain : Skill
{

    private GameObject uniquePoint;
    private Animator animator;
    public LayerMask ThisUnitLayerMask;
    private bool usingSkill = false;

    [Header("Variables")]
    public int DamagePerStack = 4;
    public int CurePerStack = 5;


    void Start()
    {
        uniquePoint = transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform
            .GetChild(0).gameObject;

        animator = uniquePoint.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (usingSkill)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetButtonDown("Fire2"))
            {
                GetComponent<battleWalk>().setSkillCommandCanvas(true);
                hideRange();
            }
            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, ThisUnitLayerMask);

            if(raycast.collider != null)
            {
                if (raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        var enemy = GameObject.Find("Enemy3x3");
                        if (enemy != null)
                        {
                            var poisonNumber = enemy.GetComponent<Unit>().checkPoison(enemy);
                            enemy.GetComponent<Unit>().TakeDamage(DamagePerStack * poisonNumber, elements.NEUTRO);
                            this.GetComponent<Unit>().cureHP(poisonNumber * CurePerStack);
                        }
                    }
                }
            }
            else
            {
                animator.SetBool("slashOver", false);
            }
        }    
    }
    public void Attack()
    {
        uniquePoint.SetActive(true);
        usingSkill = true;
    }
    void hideRange()
    {
        uniquePoint.SetActive(false);
        usingSkill = false;
    }


}
