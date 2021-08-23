using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeImageCode : MonoBehaviour
{
    private bool initialFade, firstGameFade, gameOverLoaded, gameStarted, resultScreen, showRating, showMessage;
    
    private bool loadIntoGame, loadEndScreen, loadGameOver, loadMenu, decreaseAlphaFinalText;

    private bool firstRating, secondRating, thirdRating, fourthRating;

    private Image fadeImage, cog1, cog3;

    private battleSystem BattleSystem;

    private float timer = 3, timer2 = 2, timer3 = 2, timer4 = 2, timer5 = 2.5f;
    
    private float ratingTimer = 1.5f, timerForEndScreen = 1.25f;

    public GameObject gameOverText, resultsGameObject;

    public TextMeshProUGUI[] allTextRatings;

    private int finalRating;

    public GameObject buttonToActivate, messageGO, extraMessageGO;
    
    void Start()
    {
        BattleSystem = GameObject.Find("BattleSystem").GetComponent<battleSystem>();
        fadeImage = gameObject.GetComponent<Image>();
        cog1 = fadeImage.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        cog3 = fadeImage.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (!initialFade)
        {
            if (timer2 > 0)
            {
                timer2 = timer2 - Time.deltaTime;
            }
            else
            {
                if (fadeImage.color.a > 0)
                {
                    DecreaseAlpha(false);
                }
                else
                {
                    initialFade = true;
                    BattleSystem.ActivateMenu();
                }
            }
        }

        if (loadIntoGame)
        {
            if (!firstGameFade)
            {
                if (fadeImage.color.a < 1)
                {
                    IncreaseAlpha(false);
                }
                else
                {
                    firstGameFade = true;
                    BattleSystem.DeactivateMenu();
                }
            }
            else
            {
                if (!gameStarted)
                {
                    if (timer > 0)
                    {
                        timer = timer - Time.deltaTime;
                    }
                    else
                    {
                        if (fadeImage.color.a > 0)
                        {
                            DecreaseAlpha(false);
                        }
                        else
                        {
                            BattleSystem.StartGame();
                            gameStarted = true;
                        }
                    }
                }
            }
        }

        if (loadGameOver)
        {
            if (!gameOverLoaded)
            {
                if (fadeImage.color.a < 1)
                {
                    IncreaseAlpha(true);
                }
                else
                {
                    gameOverText.SetActive(true);

                    if (timer4 > 0)
                    {
                        timer4 = timer4 - Time.deltaTime;
                    }
                    else
                    {
                        if (gameOverText.GetComponent<TextMeshProUGUI>().color.a < 1)
                        {
                            var copyColor = gameOverText.GetComponent<TextMeshProUGUI>().color;
                            var newColor = new Color(copyColor.r, copyColor.g, copyColor.b, copyColor.a + Time.deltaTime);
                            gameOverText.GetComponent<TextMeshProUGUI>().color = newColor;
                        }
                        else
                        {
                            gameOverLoaded = true;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("GameOverLoaded!");

                if (timer3 > 0)
                {
                    timer3 = timer3 - Time.deltaTime;
                }
                else
                {
                    if (gameOverText.GetComponent<TextMeshProUGUI>().color.a > 0)
                    {
                        var copyColor = gameOverText.GetComponent<TextMeshProUGUI>().color;
                        var newColor = new Color(copyColor.r, copyColor.g, copyColor.b, copyColor.a - Time.deltaTime);
                        gameOverText.GetComponent<TextMeshProUGUI>().color = newColor;
                    }
                    else
                    {
                        IncreaseAlpha(false);
                        if (cog1.color.a >= 1 && cog3.color.a >= 1)
                        {
                            SceneManager.LoadScene(0);
                        }
                    }
                }
            }
        }

        if (loadEndScreen)
        {
            if (timerForEndScreen > 0)
            {
                timerForEndScreen -= Time.deltaTime;
            }
            else
            {
                if (!resultScreen && CheckForEnemyAnimation() == false)
                {
                    IncreaseAlpha(true);

                    if (fadeImage.color.a >= 1)
                    {
                        resultsGameObject.SetActive(true);

                        ShowRatings();
                    }
                }
            }
        }

        if (loadMenu)
        {
            if (!showMessage)
            {
                var cont = 0;

                var battleResult = GameObject.Find("BattleResult").GetComponent<TextMeshProUGUI>();
                var brNewColor = new Color(battleResult.color.r, battleResult.color.g, battleResult.color.b, battleResult.color.a - Time.deltaTime);
                battleResult.color = brNewColor;
                
                var buttonColor = buttonToActivate.GetComponent<Button>().image.color;
                buttonToActivate.GetComponent<Button>().image.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, buttonColor.a - Time.deltaTime);
                
                var buttonTextColor = buttonToActivate.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color;
                var newButtonTextColor = new Color(buttonTextColor.r, buttonTextColor.g, buttonTextColor.b, buttonTextColor.a - Time.deltaTime);
                buttonToActivate.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = newButtonTextColor;

                if (buttonColor.a <= 0 && newButtonTextColor.a <= 0 && brNewColor.a <= 0)
                {
                    cont++;
                }
                
                for (int x = 0; x < allTextRatings.Length; x++)
                {
                    var textChildColor = allTextRatings[x].color;
                    allTextRatings[x].color = new Color(textChildColor.r, textChildColor.g, textChildColor.b, textChildColor.a - Time.deltaTime);
                    var textParentColor = allTextRatings[x].transform.parent.gameObject.GetComponent<TextMeshProUGUI>().color;
                    var textParentNewColor = new Color(textParentColor.r, textParentColor.g, textParentColor.b, textParentColor.a - Time.deltaTime);
                    allTextRatings[x].transform.parent.gameObject.GetComponent<TextMeshProUGUI>().color = textParentNewColor;
                    if (textChildColor.a <= 0 && textParentNewColor.a <= 0)
                    {
                        cont++;
                    }
                }

                if (cont >= 5)
                {
                    showMessage = true;
                }
            }
            else
            {
                if (!decreaseAlphaFinalText)
                {
                    var cont2 = 0;

                    if (allTextRatings[3].text == "S")
                    {
                        var extraMessage = extraMessageGO.GetComponent<TextMeshProUGUI>();
                        extraMessage.color = new Color(extraMessage.color.r, extraMessage.color.g, extraMessage.color.b,
                            extraMessage.color.a + Time.deltaTime);
                        BattleRating.GotSRank = true;

                        if (extraMessage.color.a >= 1)
                        {
                            cont2++;
                        }
                    }
                    else
                    {
                        cont2++;
                    }

                    var message = messageGO.GetComponent<TextMeshProUGUI>();
                    message.color = new Color(message.color.r, message.color.g, message.color.b,
                        message.color.a + Time.deltaTime);

                    if (message.color.a >= 1)
                    {
                        cont2++;
                    }

                    if (cont2 >= 2)
                    {
                        decreaseAlphaFinalText = true;
                    }
                }
                else
                {
                    if (timer5 > 0)
                    {
                        timer5 -= Time.deltaTime;
                    }
                    else
                    {
                        var cont3 = 0;

                        if (allTextRatings[3].text == "S")
                        {
                            var extraMessage = extraMessageGO.GetComponent<TextMeshProUGUI>();
                            extraMessage.color = new Color(extraMessage.color.r, extraMessage.color.g,
                                extraMessage.color.b,
                                extraMessage.color.a - Time.deltaTime);
                            if (extraMessage.color.a <= 0)
                            {
                                cont3++;
                            }
                        }
                        else
                        {
                            cont3++;
                        }

                        var message = messageGO.GetComponent<TextMeshProUGUI>();
                        message.color = new Color(message.color.r, message.color.g, message.color.b,
                            message.color.a - Time.deltaTime);

                        if (message.color.a <= 0)
                        {
                            cont3++;
                        }

                        if (cont3 >= 2)
                        {
                            IncreaseAlpha(false);
                            if (cog1.color.a >= 1 && cog3.color.a >= 1)
                            {
                                SceneManager.LoadScene(0);
                            }
                        }
                    }
                }
            }
        }
    }

    private void DecreaseAlpha(bool onlySquare)
    {
        var newAlpha = fadeImage.color.a - Time.deltaTime;
        var newColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
        fadeImage.color = newColor;

        if (onlySquare == false)
        {
            var newAlpha2 = cog1.color.a - Time.deltaTime;
            var newColor2 = new Color(cog1.color.r, cog1.color.g, cog1.color.b, newAlpha2);
            cog1.color = newColor2;

            var newAlpha3 = cog3.color.a - Time.deltaTime;
            var newColor3 = new Color(cog3.color.r, cog3.color.g, cog3.color.b, newAlpha3);
            cog3.color = newColor3;
        }
    }

    private void IncreaseAlpha(bool onlySquare)
    {
        var newAlpha = fadeImage.color.a + Time.deltaTime;
        var newColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
        fadeImage.color = newColor;

        if (onlySquare == false)
        {
            var newAlpha2 = cog1.color.a + Time.deltaTime;
            var newColor2 = new Color(cog1.color.r, cog1.color.g, cog1.color.b, newAlpha2);
            cog1.color = newColor2;

            var newAlpha3 = cog3.color.a + Time.deltaTime;
            var newColor3 = new Color(cog3.color.r, cog3.color.g, cog3.color.b, newAlpha3);
            cog3.color = newColor3;
        }
    }

    private void ShowRatings()
    {
        if (!showRating)
        {
            if (!firstRating)
            {
                if (ratingTimer > 0)
                {
                    ratingTimer -= Time.deltaTime;
                    allTextRatings[0].transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    var cont1 = 0;

                    if (BattleRating.Companion1Alive)
                    {
                        cont1++;
                    }

                    if (BattleRating.Companion2Alive)
                    {
                        cont1++;
                    }

                    if (cont1 == 2)
                    {
                        allTextRatings[0].text = "A";
                        finalRating += 3;
                    }
                    else if (cont1 == 1)
                    {
                        allTextRatings[0].text = "B";
                        finalRating += 2;
                    }
                    else if (cont1 == 0)
                    {
                        allTextRatings[0].text = "C";
                        finalRating += 1;
                    }

                    ratingTimer = 1.5f;
                    firstRating = true;
                }
            }
            
            if (!secondRating && firstRating)
            {
                if (ratingTimer > 0)
                {
                    ratingTimer -= Time.deltaTime;
                    allTextRatings[1].transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    if (BattleRating.DamageDealt <= 0)
                    {
                        allTextRatings[1].text = "A";
                        finalRating += 3;
                    }
                    else if (BattleRating.DamageDealt <= 25 && BattleRating.DamageDealt > 0)
                    {
                        allTextRatings[1].text = "B";
                        finalRating += 2;
                    }
                    else if (BattleRating.DamageDealt <= 50 && BattleRating.DamageDealt > 25)
                    {
                        allTextRatings[1].text = "C";
                        finalRating += 1;
                    }

                    ratingTimer = 1.5f;
                    secondRating = true;
                }
            }
            
            if (!thirdRating && secondRating)
            {
                if (ratingTimer > 0)
                {
                    ratingTimer -= Time.deltaTime;
                    allTextRatings[2].transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("DamageTaken: " + BattleRating.DamageTaken);
                    
                    if (BattleRating.DamageTaken <= 50 && BattleRating.DamageTaken > 25)
                    {
                        allTextRatings[2].text = "A";
                        finalRating += 3;
                    }
                    else if (BattleRating.DamageTaken <= 25 && BattleRating.DamageTaken > 0)
                    {
                        allTextRatings[2].text = "B";
                        finalRating += 2;
                    }
                    else if (BattleRating.DamageTaken <= 0)
                    {
                        allTextRatings[2].text = "C";
                        finalRating += 1;
                    }

                    thirdRating = true;
                    ratingTimer = 1.5f;
                }
            }
            
            if (!fourthRating && thirdRating)
            {
                if (ratingTimer > 0)
                {
                    ratingTimer -= Time.deltaTime;
                    allTextRatings[3].transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    if (finalRating >= 3 && finalRating < 6)
                    {
                        allTextRatings[3].text = "C";
                    }
                    else if (finalRating >= 6 && finalRating < 9)
                    {
                        allTextRatings[3].text = "B";
                    }
                    else if (finalRating >= 9)
                    {
                        allTextRatings[3].text = "S";
                    }

                    buttonToActivate.SetActive(true);
                    
                    resultScreen = true;
                    fourthRating = true;
                    showRating = true;
                }
            }
        }
    }
    
    public void LoadIntoGame()
    {
        loadIntoGame = true;
    }

    public void LoadEndScreen()
    {
        loadEndScreen = true;
    }

    public void ReturnToMenuAfterEndScreen()
    {
        loadMenu = true;
        buttonToActivate.GetComponent<Button>().interactable = false;
    }

    private bool CheckForEnemyAnimation()
    {
        var enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Unit>();

        if (enemy.playingDeathAnimation == false)
        {
            return false;
        }

        return true;
    }

    public void LoadGameOver()
    {
        loadGameOver = true;
    }
}
