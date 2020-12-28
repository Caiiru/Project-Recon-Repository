using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultStrike:Skill
{
<<<<<<< Updated upstream
    public VaultStrike(){


    }
=======
    public VaultStrike()
    {


    }

    private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;

    private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    [SerializeField] LayerMask layermask;

    [SerializeField] bool usingSkill = false;
    
>>>>>>> Stashed changes


    public void Attack(){
 Debug.Log(this.skillName);
    }
}
