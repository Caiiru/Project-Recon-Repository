using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int maxHP;
    public int currentHP;
    public int damage;
    public double charSpeed;
    public double totalSpeed;
    public int listPosition;
    public string Elemento;
    public Animator anim;
    public GameObject floatintextPrefab;

    public AudioClip somDeAtaque;
    public AudioClip somDeTomarDano;
    public AudioClip somDeAndar;
    public AudioClip somDeMorte;
    public AudioClip somDeNão;

    //Status
    int Poisonturnos;
    bool isPoisoned;



    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public bool TakeDamage(int dmg, string element)
    {

        anim.GetComponent<Animator>().SetBool("takeDamage", true);
        Invoke("resetAllAnims", 1f);
        if (this.Elemento == "Fogo")
        {
            Debug.Log("Fogo");
            switch (element)
            {
                case "Planta":
                    dmg = dmg / 2;
                    break;
                case "Agua":
                    dmg *= 2;
                    break;
                case "Neutro":
                    break;
                default:
                    break;
            }
        }
        else if (this.Elemento == "Planta")
        {
            Debug.Log("Planta");
            switch (element)
            {
                case "Fogo":
                    dmg *= 2;
                    break;
                case "Agua":
                    dmg = dmg / 2;
                    break;
                case "Neutro":

                    break;
                default:
                    break;
            }
        }
        else if (this.Elemento == "Neutro")
        {
            Debug.Log("Neutro");
            switch (element)
            {
                case "Neutro":
                    dmg *= 2;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (element)
            {
                case "Planta":
                    dmg *= 2;
                    break;
                case "Agua":
                    break;
                case "Neutro":
                    break;
                default:

                    dmg = dmg / 2;
                    break;
            }
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
    void showFloatingText(int damage)
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

    
    public void battleStatus(string Effect)
    {
        switch (Effect)
        {
            case "Poison":
                isPoisoned = true;
                Poisonturnos += 3;
                Debug.Log("Poisoned");
                break;
        }

    }

    public void checkStatus()
    {

        if(isPoisoned)
        {
            if (Poisonturnos > 0)
            {
                Poisonturnos -= 1;
                currentHP -= 3;
                Debug.Log("Faltam: "+Poisonturnos+ " Turnos");
            }
            else if(Poisonturnos > 9)
            {
                Poisonturnos = 9;
            }
            else
                isPoisoned = false;
            
        }
    }

}
