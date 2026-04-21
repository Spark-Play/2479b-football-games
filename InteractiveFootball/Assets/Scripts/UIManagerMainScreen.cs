using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerMainScreen : MonoBehaviour
{

    [SerializeField]
    GameObject[] playerCells;

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

    [Header("Misc")]

    [SerializeField]
    TMP_Text[] minigameLeaderboardEntries;
    [SerializeField]
    TMP_Text[] finalLeaderboardEntries;

    private MinigameInfo currentMinigameInfo;

    public void UpdateAttributes()
    {
        for (int i = 0; i < GameManager.instance.playerCount; i++)
        {
            playerCells[i].SetActive(true);
        }

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


    public int currentScreen = 0;

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


    public Dictionary<string, int> sortedLeaderboard = new Dictionary<string, int>();


    void SortAndUpdateLeaderboard(int[] scores, TMP_Text[] entries)
    {
        sortedLeaderboard.Clear();
        for (int i = 0; i < GameManager.instance.playerCount; i++)
        {
            sortedLeaderboard.Add(GameManager.instance.playerNames[i], scores[i]);
        }

        sortedLeaderboard = sortedLeaderboard.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        int index = 0;
        foreach (var item in sortedLeaderboard)
        {
            entries[index].text = item.Key + " | " + item.Value.ToString();

            //entries[index].text = "askjdhsakhdas";

            index++;
        }



    }

    private IEnumerator IEndOfMinigame()
    {
        SortAndUpdateLeaderboard(GameManager.instance.minigameScores, minigameLeaderboardEntries);

        NextScreen();
        yield return new WaitForSeconds(2f);
        NextScreen();
        yield return new WaitForSeconds(2f);
        NextScreen();
        NextScreen();

        yield return new WaitForSeconds(5f);
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
        currentScreen = 1;

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

        NextScreen();

        yield return new WaitForSeconds(1.5f);
        GameManager.instance.LoadMinigame();
        yield return new WaitForSeconds(0.5f);

        GameManager.instance.ResetScoreStreak();

        mainScreens[currentScreen].SetActive(false);

        //IMinigameController.instance.MinigameIntro();


    }

    public void EndGame()
    {
        StartCoroutine(IEndGame());
    }


    IEnumerator IEndGame()
    {

        SortAndUpdateLeaderboard(GameManager.instance.minigameScores, minigameLeaderboardEntries);

        NextScreen();
        yield return new WaitForSeconds(2f);
        NextScreen();
        yield return new WaitForSeconds(2f);


        SortAndUpdateLeaderboard(GameManager.instance.totalScores, finalLeaderboardEntries);

        NextScreen();
        NextScreen();

        yield return new WaitForSeconds(5f);
        NextScreen();
        GameManager.instance.companionScreen.EndGame(sortedLeaderboard);
        yield return new WaitForSeconds(5f);
        NextScreen();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }


}
