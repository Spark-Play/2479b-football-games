using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMainScreen : MonoBehaviour
{

    [SerializeField]
    GameObject[] mainScreens;

    [Header("Dynamic Attributes")]
    [Header("Instructions")]
    [SerializeField]
    TMP_Text instructionsObjective;

    [Header("TitleCard")]
    [SerializeField]
    TMP_Text titleCardSubtitle;
    [SerializeField]
    Image titleCardImage;


    [Header("GetReady")]
    [SerializeField]
    TMP_Text getReadySubtitle;
    [SerializeField]
    TMP_Text getReadyCountdown;

    private MinigameInfo currentMinigameInfo;

    public void UpdateAttributes()
    {
        currentMinigameInfo = GameManager.instance.minigameInfos[GameManager.instance.currentGamemode];

        //instructionsTitle.text = currentMinigameInfo.name;
        instructionsObjective.text = currentMinigameInfo.description;
        //instructionsFirstUp.text = "First Up: " + GameManager.instance.playerNames[GameManager.instance.currentPlayer];

        //titleCardTitle.text = currentMinigameInfo.name;
        titleCardSubtitle.text = currentMinigameInfo.subtitle;
        titleCardImage.sprite = currentMinigameInfo.logo;

        //getReadySubtitle.text =  $"{GameManager.instance.playerNames[GameManager.instance.currentPlayer]} | {currentMinigameInfo.name}";
        //getReadyTotalScore.text = $"Total Score: {GameManager.instance.totalScores[GameManager.instance.currentPlayer]}";

    }


    int currentScreen = 0;

    public void NextScreen()
    {
        mainScreens[currentScreen].SetActive(false);
        currentScreen++;
        mainScreens[currentScreen].SetActive(true);
    }


    public void EndOfMinigame()
    {
        StartCoroutine(IEndOfMinigame());
    }

    private IEnumerator IEndOfMinigame()
    {
        NextScreen();
        yield return new WaitForSeconds(2f);
        NextScreen();
        yield return new WaitForSeconds(2f);
        NextScreen();
        NextScreen();

        yield return new WaitForSeconds(2f);
        FirstTimeMinigameTransition();
    }
     
    public void EndOfPlayerTurn()
    {
        StartCoroutine(IEndOfPlayerTurn());
    }

    private IEnumerator IEndOfPlayerTurn()
    {
        NextScreen();
        yield return new WaitForSeconds(2f);
        NextScreen();
        yield return new WaitForSeconds(2f);
        NextScreen();
        yield return new WaitForSeconds(2f);

        MinigameTransition();

    }

    public void FirstTimeMinigameTransition()
    {
        StartCoroutine(IFirstTimeMinigameTransition());
    }

    IEnumerator IFirstTimeMinigameTransition()
    {
        mainScreens[currentScreen].SetActive(false);
        currentScreen = 0;

        NextScreen();
        yield return new WaitForSeconds(2f);

        NextScreen();

        yield return new WaitForSeconds(2f);

        NextScreen();

        yield return new WaitForSeconds(2f);

        MinigameTransition();

    }

    public void MinigameTransition()
    {
        StartCoroutine(IMinigameTransition());
    }

    IEnumerator IMinigameTransition()
    {

        mainScreens[currentScreen].SetActive(false);

        GameManager.instance.individualScore = 0;

        currentScreen = 3;
        GameManager.instance.LoadMinigame();

        NextScreen();

        yield return new WaitForSeconds(2f);

        NextScreen();


        StartCoroutine(GetReadyCountdown(3));

    }

    IEnumerator GetReadyCountdown(int countdownLength)
    {

        for (int i = countdownLength; i > 0; i--)
        {
            getReadyCountdown.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        getReadyCountdown.text = "GO!";


        yield return new WaitForSeconds(1f);

        mainScreens[currentScreen].SetActive(false);

        GameManager.instance.StartMinigame();
    }


}
