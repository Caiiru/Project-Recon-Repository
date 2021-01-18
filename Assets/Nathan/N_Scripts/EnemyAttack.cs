using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttack : MonoBehaviour
{
    public Tilemap attackTilemap;

    public Vector3 origin;

    public float damage;

    public int duration;

    public int priority;
}
