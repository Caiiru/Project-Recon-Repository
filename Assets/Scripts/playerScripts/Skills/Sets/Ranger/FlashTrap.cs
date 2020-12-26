using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTrap : Skill
{
    public FlashTrap(){
        
    }

    public void Attack(){
        Debug.Log(this.skillName);
    }
}
