using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class c_action : MonoBehaviour
{
    public GameObject HudPanel;
    public GameObject OutSqr;
    [SerializeField] private float inSpeed =3;
    public Transform pos_inSqr;
    private bool walking;

    public bool IsActive;
    
    bool acertou;
    bool returnErrou;
    bool returnAcertou;

    private battleWalk _owner;

    void Update()
    {
        if (_owner != null)
        {
            if (walking)
            {
                transform.position = new Vector2(transform.position.x + inSpeed * Time.deltaTime, transform.position.y);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (acertou)
                {
                    returnAcertou = acertou;
                }
                else
                {
                    returnErrou = true;
                    returnAcertou = false;
                }
                
                walking = false;
            }
        }
    }

    public void Resetar()
    {
        walking = false;
        transform.position = pos_inSqr.transform.position;
        HudPanel.gameObject.SetActive(false);
        returnAcertou = false;
        returnErrou = false;
        _owner = null;
        IsActive = false;
    }

    public bool ReturnWalking()
    {
        return walking;
    }

    public void TurnOffHud()
    {
        HudPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if(collision.gameObject.name =="outSqr")
        {    
            acertou = true;
        }
        if(collision.gameObject.name == "inSqrFinalPosition")
        {
            walking = false;

            returnErrou = true;
        }    
    }

    public void SetOwner(battleWalk Owner)
    {
        _owner = Owner;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        acertou = false;
    }

    public void Ativar()
    {
        IsActive = true;
        HudPanel.gameObject.SetActive(true);
        StartCoroutine(walkingAtivar());
    }
    IEnumerator walkingAtivar()
    {
        yield return new WaitForSeconds(.5f);
        walking = true;
    }
    public bool Retornar()
    {
        return returnAcertou;
    }
    public bool RetornarErro()
    {
        return returnErrou;
    }

    public battleWalk ReturnOwner()
    {
        return _owner;
    }
}
