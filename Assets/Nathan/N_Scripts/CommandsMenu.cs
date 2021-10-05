using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandsMenu : MonoBehaviour
{
    private TextMeshProUGUI descriptionText, descriptionTitle;
    
    public Button[] allButtons = new Button[1];

    public ButtonEffects[] buttonsEffects;

    private GameObject commandDesc;

    void Start()
    {
        commandDesc = gameObject.transform.GetChild(2).gameObject;
        
        descriptionText = gameObject.transform.GetChild(2).transform.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        
        descriptionTitle = gameObject.transform.GetChild(2).transform.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        
        descriptionText.gameObject.SetActive(false);
        
        descriptionTitle.gameObject.SetActive(false);
        
        commandDesc.SetActive(false);
        
        var allChildren = gameObject.transform.GetChild(1).transform.GetComponentsInChildren<Button>();

        var buttonIndex = 0;
        
        for (int x = 0; x < allChildren.Length; x++)
        {
            if (allChildren[x].interactable)
            {
                if (allButtons[0] != null)
                {
                    var copyArray = new Button[allButtons.Length];

                    for (int y = 0; y < copyArray.Length; y++)
                    {
                        copyArray[y] = allButtons[y];
                    }
                    
                    allButtons = new Button[copyArray.Length + 1];

                    for (int z = 0; z < allButtons.Length; z++)
                    {
                        if (z < copyArray.Length)
                        {
                            allButtons[z] = copyArray[z];
                        }
                        else
                        {
                            allButtons[z] = allChildren[x];
                        }
                    }
                }
                else
                {
                    allButtons[buttonIndex] = allChildren[x];
                    buttonIndex++;
                }
            }
        }

        buttonsEffects = new ButtonEffects[allButtons.Length];
        
        for (int x = 0; x < buttonsEffects.Length; x++)
        {
            buttonsEffects[x] = allButtons[x].GetComponent<ButtonEffects>();
        }
    }

    private void Update()
    {
        UpdateButtons();
        
        var chose = false;
        
        for (int x = 0; x < buttonsEffects.Length; x++)
        {
            if (buttonsEffects[x].ReturnIfButtonSelected())
            {
                descriptionText.gameObject.SetActive(true);
                descriptionText.text = buttonsEffects[x].Description;
                descriptionTitle.gameObject.SetActive(true);
                descriptionTitle.text = buttonsEffects[x].Title;
                commandDesc.SetActive(true);
                chose = true;
                break;
            }
        }

        if (chose == false)
        {
            descriptionText.gameObject.SetActive(false);
            descriptionTitle.gameObject.SetActive(false);
            commandDesc.SetActive(false);
        }
    }

    public void UpdateButtons()
    {
        var copyArray = new ButtonEffects[buttonsEffects.Length];

        for (int x = 0; x < copyArray.Length; x++)
        {
            if (buttonsEffects[x].gameObject.GetComponent<Button>().interactable)
            {
                copyArray[x] = buttonsEffects[x];
            }
        }
        
        var cont = 0;
        
        for (int x = 0; x < copyArray.Length; x++)
        {
            if (copyArray[x] != null)
            {
                cont++;
            }
        }

        buttonsEffects = new ButtonEffects[cont];

        var index = 0;
        
        for (int x = 0; x < copyArray.Length; x++)
        {
            if (copyArray[x] != null)
            {
                buttonsEffects[index] = copyArray[x];
                index++;
            }
        }
    }

    public void MakeThePlayerMove(GameObject unityToMove)
    {
        unityToMove.GetComponent<battleWalk>().ChangeMoveBool(true);
    }
}
