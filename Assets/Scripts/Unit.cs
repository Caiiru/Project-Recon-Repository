using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public enum elements { FOGO, PLANTA, AGUA, NEUTRO }
public class Unit : MonoBehaviour
{
    public bool PoisonButton = false;
    public int effectIndex;


    public string unitName;
    public float maxHP;
    public float currentHP;
    public int damage;
    public double charSpeed;
    public double totalSpeed;
    public int listPosition;
    public int AgressionNumber;
    public string Elemento;
    public Animator anim;
    public GameObject floatintextPrefab;
    public bool unitHasPlayed;
    
    //status

    public elements element;
    public int currentTurn;
    private status state;
    public List<status> states = new List<status>();
    public ArrayList effects = new ArrayList();

    //sons
    public AudioClip somDeAtaque;
    public AudioClip somDeTomarDano;
    public AudioClip somDeAndar;
    public AudioClip somDeMorte;
    public AudioClip somDeNão;
    public AudioClip somDeCharge;
    //
    private int damageBonusModifier = 1;

    public bool playingDeathAnimation;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (gameObject.name == "player")
            {
                damage = 50;
            }
        }
        if (PoisonButton)
        {
            PoisonButton = false;
            AddStatusEffect(effectIndex);
        }
    }

    public void AddStatusEffect(int statusIndex)
    {
        /* 1- Poison
         * 2- Slow
         * 3- Burn
         * 4- Stun
         * 5- Enraizamento
         * 6- Mark
         */
        //states.Add(state.generateStatus(statusIndex, currentTurn, this.gameObject));
        state.generateStatus(statusIndex, currentTurn, this.gameObject);

        
    }

    public void AddStatusEffect(int statusIndex, int howMuch)
    {
        state.generateStatus(statusIndex, currentTurn, this.gameObject, howMuch);
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        state = new status();
        
    }
    public bool TakeDamage(float dmg, elements elementAttack)
    {
        anim.GetComponent<Animator>().SetBool("takeDamage", true);
        Invoke("resetAllAnims", 1f);
        dmg *= damageBonusModifier;
        if (this.element == elements.FOGO) //sou de fogo
        {

            if (elementAttack == elements.PLANTA)
                dmg = (dmg / 2);
            else if (elementAttack == elements.AGUA)
                dmg = (dmg * 2);
            else if (elementAttack == elements.NEUTRO)
                dmg = dmg * 1;
            else
                dmg = dmg * 1 ;
        
        }
        else if(this.element == elements.AGUA) // sou de agua
        {
            if (elementAttack == elements.PLANTA)
                dmg = (dmg * 2);
            else if (elementAttack == elements.AGUA)
                dmg  = dmg * 1;
            else if (elementAttack == elements.NEUTRO)
                dmg = dmg*1;
            else
                dmg = (dmg/2);
        }

        else if (this.element == elements.PLANTA) // sou de planta
        {
            if (elementAttack == elements.PLANTA)
                dmg = dmg * 1;
            else if (elementAttack == elements.AGUA)
                dmg = (dmg/2);
            else if (elementAttack == elements.NEUTRO)
                dmg = dmg * 1;
            else
                dmg = (dmg * 2);
        }
        else if (this.element == elements.NEUTRO) // sou Neutro
        {
            if (elementAttack == elements.NEUTRO)
                dmg = (dmg*2);
            else
                dmg = dmg * 1;
        }
        if (dmg < 1)
        {
            dmg = 1;
        }
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

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    void showFloatingText(float damage)
    {
        var go = Instantiate(floatintextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = damage.ToString();
    }


    public void playSound(int index)
    {
        gameObject.GetComponent<AudioSource>().time = 0;

        switch (index)
        {
            case 0:
                gameObject.GetComponent<AudioSource>().clip = somDeAndar;
                gameObject.GetComponent<AudioSource>().time = 2;
                break;
            case 1:
                gameObject.GetComponent<AudioSource>().clip = somDeAtaque;
                break;
            case 2:
                gameObject.GetComponent<AudioSource>().clip = somDeTomarDano;
                break;
            case 3:
                gameObject.GetComponent<AudioSource>().clip = somDeMorte;
                break;
            case 4:
                gameObject.GetComponent<AudioSource>().clip = somDeNão;
            break;
            case 5:
                gameObject.GetComponent<AudioSource>().clip = somDeCharge;
            break;
        }

        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            stopSound();
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void stopSound()
    {
        gameObject.GetComponent<AudioSource>().Stop();
    }

    public void isAttacking()
    {
        anim.GetComponent<Animator>().SetBool("isAttacking", true);
        Invoke("resetAllAnims", 1f);
    }
    void resetAllAnims()
    {
        anim.GetComponent<Animator>().SetBool("takeDamage", false);
        anim.GetComponent<Animator>().SetBool("isAttacking", false);
    }

    public void CheckStatus()
    {
        Debug.Log(name + " checking status");
        state.CheckStatus(this.gameObject);
    }
    public int checkPoison(GameObject GO)
    {
        return state.CheckPoison(GO);
    }

    public void changeDamageModifier(int newNumber)
    {
        damageBonusModifier = newNumber;
    }
   

}
