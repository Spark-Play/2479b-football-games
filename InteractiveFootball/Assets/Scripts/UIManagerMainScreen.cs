using System;
using System.Collections;
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

    public void FirstTimeMinigameTransition()
    {
        StartCoroutine(IFirstTimeMinigameTransition());
    }

    IEnumerator IFirstTimeMinigameTransition()
    {
        currentScreen = 0;

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
        currentScreen = 2;
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
