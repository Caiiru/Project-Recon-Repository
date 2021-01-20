using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttack : MonoBehaviour
{
    public Tilemap attackTilemap;

    public GameObject[] afterEffect;

    public string attackType;
    
    public float damage;

    public int duration;

    public int priority;
}
