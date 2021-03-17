using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttack : MonoBehaviour
{
    public Tilemap attackTilemap;

    public GameObject[] afterEffect;

    public string attackType, spawnPoint;
    
    public float damage;

    public int duration;

    public int priority;

    public int strategyLevel;

    public void StrategyLevelOperation(int operationIndex)
    {
        switch (operationIndex)
        {
            case 0:
                //ADDD
                operationIndex += 1;
            break;
            case 1:
                //DECREASE
                operationIndex -= 1;
            break;
        }
    }

    public int ReturnStrategyLevel()
    {
        return strategyLevel;
    }
}
