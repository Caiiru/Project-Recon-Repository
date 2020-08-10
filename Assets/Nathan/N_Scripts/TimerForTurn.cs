using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerForTurn : MonoBehaviour
{
    private bool iniciar;

    private float tempoPrivate;

    private bool sinal;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (iniciar)
        {
            tempoPrivate = tempoPrivate - Time.deltaTime;
            if (tempoPrivate <= 0)
            {
                sinal = true;
            }
        }
    }

    public void Iniciar(float tempo)
    {
        if (iniciar != true)
        {
            tempoPrivate = tempo;
            iniciar = true;
        }
    }

    public bool Sinalizar()
    {
        return sinal;
    }

    public void Reiniciar()
    {
        iniciar = false;
        sinal = false;
    }
}
