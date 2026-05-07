using Rive.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManagerMainScreen : MonoBehaviour
{

    [SerializeField]
    GameObject[] playerCells;

    [SerializeField]
    GameObject[] mainScreens;

    [Header("Dynamic Attributes")]
    [Header("Instructions")]
    [SerializeField]
    Image instructionsObjective;
    [SerializeField]
    VideoPlayer previewVideo;

    [Header("TitleCard")]
    [SerializeField]
    TMP_Text titleCardSubtitle;
    [SerializeField]
    Image titleCardImage;
    [SerializeField]
    GameObject[] riveLogos;
    [SerializeField]
    GameObject logoPanel;


    [Header("GetReady")]
    [SerializeField]
    Image getReadyPlayerCell;

    [Header("Misc")]

    [SerializeField]
    RiveWidget transitionDoorsRive;
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

        getReadyPlayerCell.color = GameManager.instance.playerColours[GameManager.instance.currentPlayer];

        //instructionsTitle.text = currentMinigameInfo.name;
        instructionsObjective.sprite = currentMinigameInfo.description;
        //instructionsFirstUp.text = "First Up: " + GameManager.instance.playerNames[GameManager.instance.currentPlayer];

        //titleCardTitle.text = currentMinigameInfo.name;
        titleCardSubtitle.text = currentMinigameInfo.subtitle;
        titleCardImage.sprite = currentMinigameInfo.logo;


        previewVideo.clip = currentMinigameInfo.previewClip;

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

    public void SetScreen(int index)
    {
        mainScreens[currentScreen].SetActive(false);
        currentScreen = index;
        mainScreens[currentScreen].SetActive(true);
    }


    public void EndOfMinigame()
    {
        StartCoroutine(IEndOfMinigame());
    }


    public Dictionary<int, int> sortedLeaderboard = new Dictionary<int, int>();


    void SortAndUpdateLeaderboard(int[] scores, TMP_Text[] entries)
    {
        sortedLeaderboard.Clear();
        for (int i = 0; i < GameManager.instance.playerCount; i++)
        {
            sortedLeaderboard.Add(i, scores[i]);
        }

        sortedLeaderboard = sortedLeaderboard.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        int index = 0;
        foreach (var item in sortedLeaderboard)
        {
            entries[index].color = GameManager.instance.playerColours[item.Key];

            entries[index].text = "P" + (item.Key+1).ToString() + " " + GameManager.instance.playerNames[item.Key].Replace(" ", "") + " " + item.Value.ToString();

            //entries[index].text = "askjdhsakhdas";

            index++;
        }



    }

    private IEnumerator IEndOfMinigame()
    {
        SortAndUpdateLeaderboard(GameManager.instance.minigameScores, minigameLeaderboardEntries);
        

        //Retrieve Balls
        NextScreen();
        NextScreen();
        yield return new WaitForSeconds(2f);

        //Leaderboard
        NextScreen();
        NextScreen();

        yield return new WaitForSeconds(5f);

        transitionDoorsRive.StateMachine.ViewModelInstance.GetBooleanProperty("triggerOutro").Value = false;

        yield return new WaitForSeconds(2.5f);
        print(GameManager.instance.currentGamemode);

        GameManager.instance.UnloadMinigame();



        GameManager.instance.currentPlayer = 0;
        GameManager.instance.currentGamemode = (GameManager.instance.currentGamemode == 3) ? 0 : GameManager.instance.currentGamemode + 1;
        GameManager.instance.UpdateAttributes();

        Instantiate(riveLogos[GameManager.instance.currentGamemode], logoPanel.transform);

        FirstTimeMinigameTransition();
    }
     
    public void EndOfPlayerTurn()
    {
        StartCoroutine(IEndOfPlayerTurn());
    }

    private IEnumerator IEndOfPlayerTurn()
    {
        //Time!
        NextScreen();
        //Retrieve Balls
        NextScreen();
        yield return new WaitForSeconds(4f);


        transitionDoorsRive.StateMachine.ViewModelInstance.GetBooleanProperty("triggerOutro").Value = false;

        yield return new WaitForSeconds(2.5f);
        GameManager.instance.UnloadMinigame();

        //NextScreen();
        //yield return new WaitForSeconds(2f);


        GameManager.instance.currentPlayer++;
        GameManager.instance.UpdateAttributes();

        MinigameTransition();

    }

    public void FirstTimeMinigameTransition()
    {
        StartCoroutine(IFirstTimeMinigameTransition());
    }

    IEnumerator IFirstTimeMinigameTransition()
    {
        transitionDoorsRive.gameObject.SetActive(true);
        //transitionDoorsRive.StateMachine.ViewModelInstance.GetBooleanProperty("triggerOutro").Value = false;

        Instantiate(riveLogos[GameManager.instance.currentGamemode], logoPanel.transform);


        if(GameManager.instance.currentGamemode == 0) yield return new WaitForSeconds(2.5f);

        mainScreens[currentScreen].SetActive(false);

        //Objectives Screen
        SetScreen(3);
        yield return new WaitForSeconds(10f);

        //Logo Screen
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

        //mainScreens[currentScreen].SetActive(false);

        GameManager.instance.individualScore = 0;



        GameManager.instance.LoadMinigame();


        yield return new WaitForSeconds(0.8f);


        transitionDoorsRive.StateMachine.ViewModelInstance.GetBooleanProperty("triggerOutro").Value = true;
        mainScreens[currentScreen].SetActive(false);
        //Logo Screen
        SetScreen(5);
        GameManager.instance.ResetScoreStreak();



        yield return new WaitForSeconds(5f);

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
