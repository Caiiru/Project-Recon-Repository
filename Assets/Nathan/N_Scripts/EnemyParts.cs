using UnityEngine;

public class EnemyParts : MonoBehaviour
{
    private Unit[] partesDoInimigo;

    public GameObject[] directionalTilemap;
    
    public string facingDirection;
    
    void Start()
    {
        partesDoInimigo = gameObject.GetComponentsInChildren<Unit>();
        partesDoInimigo[0] = partesDoInimigo[1];
        partesDoInimigo[1] = partesDoInimigo[2];
        partesDoInimigo[2] = partesDoInimigo[3];
        partesDoInimigo[3] = partesDoInimigo[4];
        partesDoInimigo[4] = partesDoInimigo[5];
    }
    
    void Update()
    {
        for (int x = 0; x < partesDoInimigo.Length; x++)
        {
            if (partesDoInimigo[x].GetComponent<Unit>().currentHP <= 0)
            {
                partesDoInimigo[x].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            }
        }

        SetEnemyPartsPositions();
    }

    public Unit[] ReturnAllEnemyParts()
    {
        return partesDoInimigo;
    }

    private void SetEnemyPartsPositions()
    {
        switch (facingDirection)
        {
            case "RIGHT":
                MoveTowards(partesDoInimigo[0], new Vector3(0, 0.5f, 0));
                MoveTowards(partesDoInimigo[1], new Vector3(1, 0, 0));
                MoveTowards(partesDoInimigo[2], new Vector3(-1, 0, 0));
                MoveTowards(partesDoInimigo[3], new Vector3(0, -0.5f, 0));
                MoveTowards(partesDoInimigo[4], new Vector3(-1, 0.5f, 0));
                ChangeDirectionalTilemap(0);
            break;
            case "UP":
                MoveTowards(partesDoInimigo[0], new Vector3(-1, 0, 0));
                MoveTowards(partesDoInimigo[1], new Vector3(0, 0.5f, 0));
                MoveTowards(partesDoInimigo[2], new Vector3(0, -0.5f, 0));
                MoveTowards(partesDoInimigo[3], new Vector3(1, 0, 0));
                MoveTowards(partesDoInimigo[4], new Vector3(-1, -0.5f, 0));
                ChangeDirectionalTilemap(1);
            break;
            case "LEFT":
                MoveTowards(partesDoInimigo[0], new Vector3(0, -0.5f, 0));
                MoveTowards(partesDoInimigo[1], new Vector3(-1, 0, 0));
                MoveTowards(partesDoInimigo[2], new Vector3(1, 0, 0));
                MoveTowards(partesDoInimigo[3], new Vector3(0, 0.5f, 0));
                MoveTowards(partesDoInimigo[4], new Vector3(1, -0.5f, 0));
                ChangeDirectionalTilemap(2);
            break;
            case "DOWN":
                MoveTowards(partesDoInimigo[0], new Vector3(1, 0, 0));
                MoveTowards(partesDoInimigo[1], new Vector3(0, -0.5f, 0));
                MoveTowards(partesDoInimigo[2], new Vector3(0, 0.5f, 0));
                MoveTowards(partesDoInimigo[3], new Vector3(-1, 0, 0));
                MoveTowards(partesDoInimigo[4], new Vector3(1, 0.5f, 0));
                ChangeDirectionalTilemap(3);
            break;
        }
    }

    private void MoveTowards(Unit obj, Vector3 posToGo)
    {
        obj.transform.localPosition = posToGo;
    }
    
    private void ChangeDirectionalTilemap(int toActivate)
    {
        for(int x = 0; x < directionalTilemap.Length; x++)
        {
            if(x == toActivate)
            {
                directionalTilemap[x].SetActive(true);
            }
            else
            {
                directionalTilemap[x].SetActive(false);
            }
        }
    }
}
