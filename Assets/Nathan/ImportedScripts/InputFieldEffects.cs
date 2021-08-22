using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldEffects : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler
{
    private InputField _thisInputField;
    
    public AudioClip onSelect;

    public AudioClip onClick;
    
    private AudioSource _thisAudioSource;

    private bool changeMenuState, afterClick;
    
    void Start()
    {
        _thisInputField = gameObject.GetComponent<InputField>();
        _thisAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_thisInputField.interactable && gameObject.activeSelf)
        {
            gameObject.transform.localScale = gameObject.transform.localScale * 1.05f;
            
            if (changeMenuState == false)
            {
                PlayAudioSource(onSelect);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_thisInputField.interactable && gameObject.activeSelf)
        {
            var scaleX = gameObject.transform.localScale.x;

            if (scaleX > 1)
            {
                gameObject.transform.localScale = gameObject.transform.localScale / 1.05f;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_thisInputField.interactable && gameObject.activeSelf)
        {
            PlayAudioSource(onClick);
            afterClick = true;
        }
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        if (_thisInputField.interactable && gameObject.activeSelf)
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
