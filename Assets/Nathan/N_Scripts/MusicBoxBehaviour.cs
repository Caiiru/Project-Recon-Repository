using UnityEngine;

public class MusicBoxBehaviour : MonoBehaviour
{
    private AudioSource _audioSource;
    
    public AudioClip[] allMusics;

    private battleSystem _battleSystem;
    
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _battleSystem = GameObject.Find("BattleSystem").GetComponent<battleSystem>();
    }

    void Update()
    {
        if (_battleSystem.ReturnInitialFade())
        {
            if (_battleSystem.ReturnWon())
            {
                if (_audioSource.clip != allMusics[2])
                {
                    _audioSource.clip = allMusics[2];
                    _audioSource.Play();
                }
            }
            else if(_battleSystem.ReturnLost())
            {
                if (_audioSource.clip != allMusics[3])
                {
                    _audioSource.clip = allMusics[3];
                    _audioSource.Play();
                }
            }
            else if (_battleSystem.ReturnOutOfMenu())
            {
                if (_audioSource.clip != allMusics[1])
                {
                    _audioSource.clip = allMusics[1];
                    _audioSource.Play();
                }
            }
            else
            {
                if (_audioSource.clip != allMusics[0])
                {
                    _audioSource.clip = allMusics[0];
                    _audioSource.Play();
                }
            }
        }
    }
}
