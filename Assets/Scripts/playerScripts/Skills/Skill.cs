using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName;
    public int skillCD;
    private int actualCD;
    public float skillDamage;
    public bool SkillUsedThisTurn;
    
    public void SetCD()
    {
        actualCD = skillCD;
    }
    
    public void DecreaseCD()
    {
        if(actualCD > 0)
        {
            actualCD--;
        }
    }

    public int ReturnCDNumber()
    {
        return actualCD;
    }

    public bool ReturnCanUseSkill()
    {
        if (actualCD <= 0)
        {
            return true;
        }

        return false;
    }
    
}
