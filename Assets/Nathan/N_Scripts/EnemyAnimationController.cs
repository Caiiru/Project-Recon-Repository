using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Sprite[] allMonsterFrontSprites;

    public Sprite[] allMonsterBackSprites;

    public SpriteRenderer[] allMonsterParts;

    private Vector2[] partsPosFrontRight, partsPosFrontLeft, partsPosBackLeft, partsPosBackRight;

    private EnemyBattleWalk _enemyBattleWalk;
    
    void Start()
    {
        _enemyBattleWalk = gameObject.transform.parent.gameObject.GetComponent<EnemyBattleWalk>();
        partsPosFrontRight = new Vector2[11]; 
        partsPosFrontLeft = new Vector2[11];
        partsPosBackLeft = new Vector2[11];
        partsPosBackRight = new Vector2[11];
        
        partsPosFrontRight[0] = new Vector2(2.6f, 0); //BODY
        partsPosFrontRight[1] = new Vector2(2.32f, 0.04f); //NECK
        partsPosFrontRight[2] = new Vector2(2.35f, 0.19f); //HEAD
        partsPosFrontRight[3] = new Vector2(2.31f, 0.25f); //MOUTH
        partsPosFrontRight[4] = new Vector2(2.78f, -0.17f); //FRONT RIGHT LEG UPPER 
        partsPosFrontRight[5] = new Vector2(2.7f, 0.06f); //FRONT RIGHT LEG LOWER 
        partsPosFrontRight[6] = new Vector2(2.22f, -0.09f); //BACK RIGHT LEG UPPER 
        partsPosFrontRight[7] = new Vector2(2.33f, -0.16f); //BACK RIGHT LEG LOWER
        partsPosFrontRight[8] = new Vector2(2.43f, -0.07f); //FRONT LEFT LEG UPPER
        partsPosFrontRight[9] = new Vector2(2.39f, -0.07f); //FRONT LEFT LEG LOWER
        partsPosFrontRight[10] = new Vector2(2.14f, 0.05f); //TAIL
        
        partsPosFrontLeft[0] = new Vector2(-3.16f, -0.16f); //BODY
        partsPosFrontLeft[1] = new Vector2(-2.89f, -0.14f); //NECK
        partsPosFrontLeft[2] = new Vector2(-2.79f, 0.01f); //HEAD
        partsPosFrontLeft[3] = new Vector2(-2.83f, 0.07f); //MOUTH
        partsPosFrontLeft[4] = new Vector2(-3.33f, -0.13f); //FRONT RIGHT LEG UPPER
        partsPosFrontLeft[5] = new Vector2(-3.23f, 0.09f); //FRONT RIGHT LEG LOWER
        partsPosFrontLeft[6] = new Vector2(-2.73f, -0.13f); //BACK RIGHT LEG UPPER
        partsPosFrontLeft[7] = new Vector2(-2.82f, -0.17f); //BACK RIGHT LEG LOWER 
        partsPosFrontLeft[8] = new Vector2(-3.02f, -0.13f); //FRONT LEFT LEG UPPER
        partsPosFrontLeft[9] = new Vector2(-2.93f, -0.07f); //FRONT LEFT LEG LOWER 
        partsPosFrontLeft[10] = new Vector2(-2.99f, 0.21f); //TAIL
        
        partsPosBackLeft[0] = new Vector2(0, -0.16f); //BODY
        partsPosBackLeft[1] = new Vector2(0, 0); //NECK
        partsPosBackLeft[2] = new Vector2(0, -0.1f); //HEAD
        partsPosBackLeft[3] = new Vector2(0, -0.11f); //MOUTH
        partsPosBackLeft[4] = new Vector2(0, 0); //FRONT RIGHT LEG UPPER
        partsPosBackLeft[5] = new Vector2(0, 0); //FRONT RIGHT LEG LOWER
        partsPosBackLeft[6] = new Vector2(0, -0.08f); //BACK RIGHT LEG UPPER
        partsPosBackLeft[7] = new Vector2(0.1f, -0.04f); //BACK RIGHT LEG LOWER
        partsPosBackLeft[8] = new Vector2(0, 0.013f); //FRONT LEFT LEG UPPER
        partsPosBackLeft[9] = new Vector2(0.07f, -0.046f); //FRONT LEFT LEG LOWER
        partsPosBackLeft[10] = new Vector2(-0.04f, 0.001f); //TAIL
        
        partsPosBackRight[0] = new Vector2(-0.6f, -0.16f); //BODY
        partsPosBackRight[1] = new Vector2(0, 0); //NECK
        partsPosBackRight[2] = new Vector2(-0.5f, -0.1f); //HEAD
        partsPosBackRight[3] = new Vector2(-0.5f, -0.11f); //MOUTH
        partsPosBackRight[4] = new Vector2(0, 0); //FRONT RIGHT LEG UPPER
        partsPosBackRight[5] = new Vector2(0, 0); //FRONT RIGHT LEG LOWER
        partsPosBackRight[6] = new Vector2(-0.63f, -0.08f); //BACK RIGHT LEG UPPER
        partsPosBackRight[7] = new Vector2(-0.7f, -0.01f); //BACK RIGHT LEG LOWER
        partsPosBackRight[8] = new Vector2(-0.6f, -0.06f); //FRONT LEFT LEG UPPER
        partsPosBackRight[9] = new Vector2(-0.63f, -0.046f); //FRONT LEFT LEG LOWER
        partsPosBackRight[10] = new Vector2(-0.55f, 0.001f); //TAIL
    }

    // Update is called once per frame
    void Update()
    {
        switch (_enemyBattleWalk.ReturnFacingDirection())
        {
            case "RIGHT":
                ApplySpriteChanges(true);
                FlipAllSpriteRenderers(true);
                ApplyPosChanges(0);
            break;
            case "DOWN":
                ApplySpriteChanges(true);
                FlipAllSpriteRenderers(false);
                ApplyPosChanges(1);
            break;
            case "LEFT":
                ApplySpriteChanges(false);
                FlipAllSpriteRenderers(false);
                ApplyPosChanges(2);
            break;
            case "UP":
                ApplySpriteChanges(false);
                FlipAllSpriteRenderers(true);
                ApplyPosChanges(3);
            break;
        }
    }

    private void FlipAllSpriteRenderers(bool flipX)
    {
        for (int x = 0; x < allMonsterParts.Length; x++)
        {
            allMonsterParts[x].flipX = flipX;
        }
    }

    private void ApplyPosChanges(int index)
    {
        for (int x = 0; x < allMonsterParts.Length; x++)
        {
            if (index == 0)
            {
                allMonsterParts[x].transform.localPosition = partsPosFrontRight[x];               
                if (x == 0)
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.4f, 0.4f, 1);
                }
                else
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                }
                ChangeLayerOrder(true, x);
            }
            else if (index == 1)
            {
                allMonsterParts[x].transform.localPosition = partsPosFrontLeft[x];
                if (x == 0)
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.4f, 0.4f, 1);
                }
                else
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                }
                ChangeLayerOrder(true, x);
            }
            else if (index == 2)
            {
                allMonsterParts[x].transform.localPosition = partsPosBackLeft[x];
                if (x != 10)
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.15f, 0.15f, 1);
                }
                else
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.16f, 0.16f, 1);
                }
                ChangeLayerOrder(false, x);
            }
            else if (index == 3)
            {
                allMonsterParts[x].transform.localPosition = partsPosBackRight[x];
                if (x != 10)
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.15f, 0.15f, 1);
                }
                else
                {
                    allMonsterParts[x].transform.localScale = new Vector3(0.16f, 0.16f, 1);
                }
                ChangeLayerOrder(false, x);
            }
        }
    }

    private void ChangeLayerOrder(bool isItOneOrTwo, int x)
    {
        if (isItOneOrTwo)
        {
            if (x == 10)
            {
                allMonsterParts[x].sortingOrder = 5;
            }
            else if(x == 4)
            {
                allMonsterParts[x].sortingOrder = 6;
            }
            else if(x == 0 || x == 5 || x == 7)
            {
                allMonsterParts[x].sortingOrder = 7;
            }
            else if(x == 1 || x == 3 || x == 6 || x == 8)
            {
                allMonsterParts[x].sortingOrder = 8;
            }
            else if(x == 2 || x == 9)
            {
                allMonsterParts[x].sortingOrder = 9;
            }
        }
        else
        {
            if (x == 3)
            {
                allMonsterParts[x].sortingOrder = 5;
            }
            else if(x == 2)
            {
                allMonsterParts[x].sortingOrder = 6;
            }
            else if(x == 0 || x == 9 || x == 10)
            {
                allMonsterParts[x].sortingOrder = 7;
            }
            else if(x == 6 || x == 7 || x == 8)
            {
                allMonsterParts[x].sortingOrder = 8;
            }
        }
    }

    private void ApplySpriteChanges(bool isFront)
    {
        for (int x = 0; x < allMonsterParts.Length; x++)
        {
            if (isFront)
            {
                allMonsterParts[x].sprite = allMonsterFrontSprites[x];
            }
            else
            {
                allMonsterParts[x].sprite = allMonsterBackSprites[x];
            }
        }
    }
}
