using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Fly : Skill
{
    public LayerMask ArenaLayerMask, TileMapsLayerMask;
    
    public Button SkillButton;

    public TextMeshProUGUI SkillCD;
    
    private bool usingSkill;
    
    public Tilemap map;
    
    private bool canUseSkill;
    
    // Start is called before the first frame update
    void Start()
    {
        usingSkill = false;
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
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D arenaRaycast = Physics2D.Raycast(worldMousePosition, Vector3.forward,
                    Mathf.Infinity, ArenaLayerMask);
                if (arenaRaycast && arenaRaycast.collider && arenaRaycast.collider.CompareTag("Walk"))
                {   
                    RaycastHit2D entityRaycast = Physics2D.Raycast(worldMousePosition, Vector3.back,
                        Mathf.Infinity, ~ArenaLayerMask & ~TileMapsLayerMask);

                    if (!entityRaycast)
                    {
                        Vector3Int tileCoord = map.WorldToCell(worldMousePosition);
                        Vector3 CellCenterPos = map.GetCellCenterWorld(tileCoord);
                        
                        var newPos = new Vector3(CellCenterPos.x, CellCenterPos.y, gameObject.transform.position.z);
                        
                        transform.position = newPos;
                        Debug.Log("Y");
                        SetUsingSkill(false);
                        GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(1);
                        SetCooldown();
                    }
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                GetComponent<battleWalk>().setSkillCommandCanvas(true);
                SetUsingSkill(false);
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

    public void SetUsingSkill(bool value)
    {
        usingSkill = value;
    }
}
