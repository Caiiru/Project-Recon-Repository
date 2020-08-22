using UnityEngine;

public class TimerForTurn : MonoBehaviour
{
    private bool _iniciar;

    private float _tempoPrivate;

    private bool _signal;
    
    void Update()
    {
        if (_iniciar)
        {
            _tempoPrivate = _tempoPrivate - Time.deltaTime;
            if (_tempoPrivate <= 0)
            {
                _signal = true;
            }
        }
    }

    public void Iniciar(float tempo)
    {
        if (_iniciar != true)
        {
            _tempoPrivate = tempo;
            _iniciar = true;
        }
    }

    public bool Sinalizar()
    {
        return _signal;
    }

    public void Reiniciar()
    {
        _iniciar = false;
        _signal = false;
    }
}
