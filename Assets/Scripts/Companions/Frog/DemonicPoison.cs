using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonicPoison : Skill
{
    private GameObject uniquePoint;
    private Animator animator;
    public LayerMask ThisUnitLayerMask, LayerMaskToIgnore;
    private bool usingSkill;

    void Start()
    {
        uniquePoint = transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.transform
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
            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, ThisUnitLayerMask & ~LayerMaskToIgnore);

            if (raycast.collider != null)
            {
                if (raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Unit_Frog.morePoison += 1;
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
