using UnityEngine;
using UnityEngine.Tilemaps;

public class Fly : Skill
{
    private bool usingSkill;
    
    public Tilemap map;
    
    // Start is called before the first frame update
    void Start()
    {
        usingSkill = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (usingSkill)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward,
                    Mathf.Infinity);
                if (raycast && raycast.collider && raycast.collider.CompareTag("Walk"))
                {
                    Vector3Int tileCoord = map.WorldToCell(worldMousePosition);
                    Vector3 CellCenterPos = map.GetCellCenterWorld(tileCoord);
                    
                    var newPos = new Vector3(CellCenterPos.x, CellCenterPos.y, gameObject.transform.position.z);
                    
                    transform.position = newPos;
                    Debug.Log("Y");
                    SetUsingSkill(false);
                    GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(1);
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                GetComponent<battleWalk>().setSkillCommandCanvas(true);
                SetUsingSkill(false);
            }
        }
    }

    public void SetUsingSkill(bool value)
    {
        usingSkill = value;
    }
}
