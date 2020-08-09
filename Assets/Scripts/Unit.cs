using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int maxHP;
    public int currentHP;
    public int damage;
    public int charSpeed;
    public int listPosition;
    public Animator anim;
    public GameObject floatintextPrefab;
    
    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (floatintextPrefab)
        {
            showFloatingText(dmg);
        }
        if (currentHP <= 0)
            return true;
        else
            return false;

    }

    public void cureHP(int cure)
    {
        currentHP += cure;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    void showFloatingText(int damage )
    {
        var go = Instantiate(floatintextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    public void isMyTurn()
    {
        anim.SetBool("crescendo", true);
        anim.SetBool("crescendo", false);
    }
    public void isNotMyTurn()
    {
        anim.SetBool("skulldiminuindo", true);
        anim.SetBool("skulldiminuindo", false);
    }
}
