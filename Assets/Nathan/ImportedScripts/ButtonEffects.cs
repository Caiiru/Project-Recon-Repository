using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Button _thisButton;
    
    public AudioClip onSelect;

    public AudioClip onClick;
    
    public AudioSource _thisAudioSource;

    private bool wasClicked, selected;

    public string Description;

    private FadeImageCode fadeImageCode;
    void Start()
    {
        _thisButton = gameObject.GetComponent<Button>();
        fadeImageCode = GameObject.Find("FadeImage").GetComponent<FadeImageCode>();
    }
    
    private void Update()
    {
        if (wasClicked && gameObject.name == "Begin")
        {
            var main = GameObject.Find("Main");

            var mainChilds = main.GetComponentsInChildren<Button>();

            for (int x = 0; x < mainChilds.Length; x++)
            {
                mainChilds[x].interactable = false;
            }
           
            if (!_thisAudioSource.isPlaying)
            {
                fadeImageCode.LoadIntoGame();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_thisButton.interactable && gameObject.activeSelf)
        {
            gameObject.transform.localScale = gameObject.transform.localScale * 1.05f;
            PlayAudioSource(onSelect);
            selected = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_thisButton.interactable && gameObject.activeSelf)
        {
            var scaleX = gameObject.transform.localScale.x;

            if (scaleX > 1f)
            {
                gameObject.transform.localScale = gameObject.transform.localScale / 1.05f;
            }

            selected = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_thisButton.interactable && gameObject.activeSelf)
        {
            StopAllSounds();

            var scale = gameObject.transform.localScale.x;

            if (scale > 1f)
            {
                gameObject.transform.localScale = gameObject.transform.localScale / 1.05f;
            }

            PlayAudioSource(onClick);

            wasClicked = true;

            selected = false;
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

    public bool ReturnIfButtonSelected()
    {
        return selected;
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
