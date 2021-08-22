using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleEffects : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler
{
    private Toggle _thisToggle;
    
    public AudioClip onSelect;

    public AudioClip onClick;
    
    public AudioSource _thisAudioSource;
    
    void Start()
    {
        _thisToggle = gameObject.GetComponent<Toggle>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_thisToggle.interactable && gameObject.activeSelf)
        {
            gameObject.transform.GetChild(0).gameObject.transform.localScale = gameObject.transform.GetChild(0).gameObject.transform.localScale * 1.05f;
            PlayAudioSource(onSelect);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_thisToggle.interactable && gameObject.activeSelf)
        {
            var scale = gameObject.transform.GetChild(0).gameObject.transform.localScale.x;
            
            if (scale > 1f)
            {
                gameObject.transform.GetChild(0).gameObject.transform.localScale = gameObject.transform.GetChild(0).gameObject.transform.localScale / 1.05f;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_thisToggle.interactable && gameObject.activeSelf)
        {
            StopAllSounds();
            
            var scale = gameObject.transform.GetChild(0).gameObject.transform.localScale.x;
            
            if (scale > 1f)
            {
                gameObject.transform.GetChild(0).gameObject.transform.localScale = gameObject.transform.GetChild(0).gameObject.transform.localScale / 1.05f;
            }

            PlayAudioSource(onClick);
        }
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        if (_thisToggle.interactable && gameObject.activeSelf)
        {
            Debug.Log("ButtonInteraction04");
        }
    }
    
    private void PlayAudioSource(AudioClip toPlay)
    {
        if (_thisAudioSource.isPlaying)
        {
            _thisAudioSource.Stop();
        }

        _thisAudioSource.clip = toPlay;
        
        _thisAudioSource.Play();
    }

    private void StopAllSounds()
    {
        var all = FindObjectsOfType<AudioSource>();

        for (int x = 0; x < all.Length; x++)
        {
            if (all[x].gameObject.GetComponent<AudioSource>())
            {
                all[x].gameObject.GetComponent<AudioSource>().Stop();
            }
        }
    }
}

