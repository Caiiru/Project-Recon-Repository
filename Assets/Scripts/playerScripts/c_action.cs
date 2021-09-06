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
    bool acertou;

    bool returnErrou;
    bool returnAcertou;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(walking)
        {
           transform.position = new Vector2(transform.position.x + inSpeed * Time.deltaTime, transform.position.y);
        }

        if(Input.GetButtonDown("Fire1"))
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

    public void Resetar()
    {
        transform.position = pos_inSqr.transform.position;
        HudPanel.gameObject.SetActive(false);
        returnAcertou = false;
        returnErrou = false;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
      
        acertou = false;
    }

    public void Ativar()
    {
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
}
