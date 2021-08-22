using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SETS{Scout, Ranger, Destroyer}
public class setManager : MonoBehaviour
{
    public SETS Set;
    public GameObject setMenu;
    
    private Slash slash;
    private VaultStrike vaultStrike;
    private Decapting decapting;

    private DashStrike dashStrike;

    private Prepare prepare;
    private Tackle tackle;
    private Fortress fortress;
    private SpinToWin spin;

    private FlashTrap flashTrap;
    private Mark mark;
    private PerfectShot perfectShot;
    private PiercingShot piercingShot;

    public Text skill1;
    public Text skill2;
    public Text skill3;
    public Text skill4;



    public List<Skill>skillList = new List<Skill>();

    void Start(){
        //setMenu.SetActive(true);
        slash = this.gameObject.GetComponent<Slash>();
        vaultStrike = this.gameObject.GetComponent<VaultStrike>();
        decapting = this.gameObject.GetComponent<Decapting>();
        dashStrike = this.gameObject.GetComponent<DashStrike>();

        spin = this.gameObject.GetComponent<SpinToWin>();
        prepare = this.gameObject.GetComponent<Prepare>();
        fortress = this.gameObject.GetComponent<Fortress>();
        tackle = this.gameObject.GetComponent<Tackle>();

        perfectShot = this.gameObject.GetComponent<PerfectShot>();
        piercingShot = this.gameObject.GetComponent<PiercingShot>();
        flashTrap = this.gameObject.GetComponent<FlashTrap>();
        mark = this.gameObject.GetComponent<Mark>();
        changeSet(3);
    }
    void Update(){
       
    }


    public void changeSet(int number){
        switch (number){
            case 1:
                Set = SETS.Ranger;
            break;
            case 2:
                Set = SETS.Destroyer;
            break;
            case 3:
                Set = SETS.Scout;
                
            break;
            default:
            Debug.Log("Erro");
            break;

        }
        setSkills(Set);
        setMenu.SetActive(false);
        GameObject bs = GameObject.FindGameObjectWithTag("BS");
        bs.GetComponent<battleSystem>().state = BattleState.START;
        
        
    }
    private void setSkills(SETS set){
        //Debug.Log(set);
        switch(set){
            case SETS.Destroyer:
                skillList.Add(spin);
                skillList.Add(tackle);
                skillList.Add(fortress);
                skillList.Add(prepare);
                skill1.text = spin.skillName;
                skill2.text = tackle.skillName;
                skill3.text = fortress.skillName;
                skill4.text = prepare.skillName;

            break;
            case SETS.Ranger:
                skillList.Add(mark);
                skillList.Add(piercingShot);
                skillList.Add(perfectShot);
                skillList.Add(flashTrap);
                skill1.text = flashTrap.skillName;
                skill2.text = piercingShot.skillName;
                skill3.text = mark.skillName;
                skill4.text = perfectShot.skillName;

            break;

            case SETS.Scout:
                skillList.Add(slash);
                skillList.Add(dashStrike);
                skillList.Add(vaultStrike);
                skillList.Add(decapting);
                skill1.text = slash.skillName;
                skill2.text = dashStrike.skillName;
                skill3.text = vaultStrike.skillName;
                skill4.text = decapting.skillName;
            break;

            default:
                Debug.Log("Nothing");
            break;
        }
    }


    public void firstSkill(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<battleWalk>().setSkillCommandCanvas(false);
        switch(this.Set){
            case SETS.Scout:
            slash.Attack();
            break;

            case SETS.Ranger:
            flashTrap.Attack();
            break;

            case SETS.Destroyer:
            spin.Attack();
            break;
            
        }

    }
    public void secondSkill(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<battleWalk>().setSkillCommandCanvas(false);
        switch (this.Set){
            case SETS.Scout:
            dashStrike.Attack();
            break;

            case SETS.Ranger:
            piercingShot.Attack();
            break;

            case SETS.Destroyer:
            tackle.Attack();
            break;
            
        }
    }
    public void thirdSkill(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<battleWalk>().setSkillCommandCanvas(false);
        switch (this.Set){
            case SETS.Scout:
            vaultStrike.Attack();
            break;

            case SETS.Ranger:
            mark.Attack();
            break;

            case SETS.Destroyer:
            fortress.Attack();
            break;
            
        }
    }
    public void fourthtSkill(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<battleWalk>().setSkillCommandCanvas(false);
        switch (this.Set){
            case SETS.Scout:
            decapting.Attack();
            break;

            case SETS.Ranger:
            perfectShot.Attack();
            break;

            case SETS.Destroyer:
            prepare.Attack();
            break;
            
        }
    }
}
