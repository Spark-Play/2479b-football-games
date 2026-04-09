using System;
using System.Collections;
using TMPro;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManagerMainScreen mainScreen;
    public UIManagerCompanionScreen companionScreen;

    public int currentPlayer = 0;
    public int currentGamemode = 0;
    public int playerCount = 0;
    public float progress = 0;
    public MinigameInfo[] minigameInfos;

    
    public int individualScore = 0;
    public int[] totalScores;
    public string[] playerNames;
    public int duration = 15;

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore(int pointValue)
    {
        totalScores[currentPlayer] += pointValue;
        individualScore += pointValue;
        IMinigameController.instance.scoreText.text = individualScore.ToString();
        //Update Score Text Here
    }

    public void StartSession()
    {
        companionScreen.NextScreen();
        UpdateAttributes();
        mainScreen.FirstTimeMinigameTransition();

        StartCoroutine(IStartOverallTimer());
    }

    IEnumerator IStartOverallTimer()
    {
        //yield return new WaitForSeconds(duration * 60);

        int durationInSeconds = duration * 60;


        for (int i = durationInSeconds; i >= 0; i--)
        {
            int minutes = i / 60;
            int seconds = i % 60;

            progress = (durationInSeconds - i + 0.00001f) / durationInSeconds;

            //print(progress);

            companionScreen.progressSlider.value = progress;
            companionScreen.sessionTimeRemaining.text = $"{minutes:00}:{seconds:00}";
            yield return new WaitForSeconds(1f);
        }

        EndGame();
    }

    public void BeginNewMinigame()
    {
        SceneManager.UnloadSceneAsync(minigameInfos[currentGamemode].sceneName);

        if(currentGamemode >= 3)
        {
            currentPlayer = 0;
            currentGamemode = 0;
            UpdateAttributes();
            mainScreen.EndOfMinigame();
        }
        if(currentPlayer >= playerCount-1)
        {
            currentPlayer = 0;
            currentGamemode++;
            //CalculateProgress();
            UpdateAttributes();
            mainScreen.EndOfMinigame();
        }
        else
        {
            currentPlayer++;
            //CalculateProgress();
            UpdateAttributes();
            mainScreen.EndOfPlayerTurn();
        }


        //companionScreen.progressSlider.value = 

    }


    public static event Action OnUpdateAttributes;

    void UpdateAttributes()
    {
        //companionScreen.UpdateMinigameScreen();
        mainScreen.UpdateAttributes();
        OnUpdateAttributes.Invoke();
    }

    void CalculateProgress()
    {
        float totalProgress = minigameInfos.Length * playerCount;

        float playerProgress = currentPlayer + 1;

        float minigameProgress = currentGamemode * minigameInfos.Length;

        progress = minigameProgress + playerProgress / totalProgress;
    }

    public void LoadMinigame()
    {
        SceneManager.LoadScene(minigameInfos[currentGamemode].sceneName, LoadSceneMode.Additive);
    }

    public void StartMinigame()
    {
        IMinigameController.instance.StartMinigame();
    }


    public void EndGame()
    {
        print("endgame");
    }


}
