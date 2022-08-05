using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemonicPoison : Skill
{
    public Button SkillButton;

    public TextMeshProUGUI SkillCD;
    
    private bool canUseSkill;
    
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
            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, ThisUnitLayerMask & ~LayerMaskToIgnore);

            if (raycast.collider != null)
            {
                if (raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Unit_Frog.morePoison += 1;
                        hideRange();
                        GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(2);
                        SetCooldown();
                        gameObject.GetComponent<Unit>().playSound(5);
                    }
                }
            }
            else
            {
                animator.SetBool("slashOver", false);
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
