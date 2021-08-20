using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_newSetManager : MonoBehaviour
{
    public so_baseSkill currentSkill;
    public bool FirstSkill;
    public List<so_baseSkill> skillList = new List<so_baseSkill>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FirstSkill)
            firstSkillUse();
    }

    void firstSkillUse()
    {
        
        FirstSkill = false;
        currentSkill = skillList[0];
        Debug.Log("Use " + currentSkill.name);
        currentSkill.Use();
    }
}
