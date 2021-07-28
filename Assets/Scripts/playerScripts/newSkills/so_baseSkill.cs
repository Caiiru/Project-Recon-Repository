using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class so_baseSkill : MonoBehaviour
{
    public new string name;
    public int cooldown;
    public float damage;
    public bool UseSkill;
    
    public virtual void Use() { 
    } 
}
