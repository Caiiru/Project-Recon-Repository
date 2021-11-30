using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class FrogPasive : Skill
{
    private bool spreadPoison;
    
    void Update()
    {
        if (spreadPoison)
        {
            for (int x = 0; x < 4; x++)
            {
                SpreadPoison(x);
            }
            
            spreadPoison = false;
        }
    }

    private void SpreadPoison(int index)
    {
        var poisonSpread = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        
        switch (index)
        {
            case 0:
                poisonSpread = new Vector2(poisonSpread.X + 0.5f, poisonSpread.Y - 0.25f);//DIREITA
            break;
            case 1:
                poisonSpread = new Vector2(poisonSpread.X + 0.5f, poisonSpread.Y + 0.25f);//CIMA
            break;
            case 2:
                poisonSpread = new Vector2(poisonSpread.X - 0.5f, poisonSpread.Y + 0.25f);//ESQUERDA
            break;
            case 3:
                poisonSpread = new Vector2(poisonSpread.X - 0.5f, poisonSpread.Y - 0.25f);//BAIXO
            break;
        }
        
        var newV3Pos = new Vector3(poisonSpread.X, poisonSpread.Y, gameObject.transform.position.z);
        
        RaycastHit2D hit2D = Physics2D.Raycast(newV3Pos, Vector3.back, Mathf.Infinity);
        
        Debug.DrawRay(newV3Pos, Vector3.back, Color.cyan, Mathf.Infinity);

        if (hit2D && hit2D.collider)
        {
            if (hit2D.collider.CompareTag("Enemy"))
            {
                hit2D.collider.gameObject.GetComponent<Unit>().AddStatusEffect(1 + Unit_Frog.morePoison);
            }
            else if(hit2D.collider.CompareTag("EnemyPart"))
            {
                var parent = hit2D.transform.parent.gameObject.GetComponent<Unit>();
                parent.AddStatusEffect(1);
            }
        }
    }

    public void SetSpreadPoison(bool value)
    {
        spreadPoison = value;
    }
}
