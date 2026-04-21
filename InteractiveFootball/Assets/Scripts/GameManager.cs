using System;
using System.Collections;
using TMPro;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public TcpJsonXYClient tcpJsonXYClient;

    public UIManagerMainScreen mainScreen;
    public UIManagerCompanionScreen companionScreen;

    public int currentPlayer = 0;
    public int currentGamemode = 0;
    public int playerCount = 0;
    public float progress = 0;
    public MinigameInfo[] minigameInfos;

    public int streak = 0;
    public int individualScore = 0;
    public int[] minigameScores;
    public int[] totalScores;
    public string[] playerNames;
    public int duration = 15;



    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void ResetScoreStreak()
    {
        streak = 0;
        IMinigameController.instance.streakBonusText.text = "";
    }

    public void UpdateScoreCancelStreak(int pointValue)
    {
        //Reset Streak
        streak = 0;
        IMinigameController.instance.streakBonusText.text = "";



        //Calc Net Score
        int calculatedScore = pointValue;

        totalScores[currentPlayer] += calculatedScore;
        individualScore += calculatedScore;
        IMinigameController.instance.scoreText.text = individualScore.ToString();
    }

    public void UpdateScore(int pointValue)
    {
        //Calc Streak Bonus
        int streakBonus = 0;

        if(streak < 2)
        {
            IMinigameController.instance.streakBonusText.text = "";
            streakBonus = 0;
        }
        else if(streak < 4)
        {
            streakBonus = 5;
            IMinigameController.instance.streakBonusText.text = "+" + streakBonus.ToString();
        }
        else if (streak < 6)
        {
            streakBonus = 10;
            IMinigameController.instance.streakBonusText.text = "+" + streakBonus.ToString();
        }
        else
        {
            streakBonus = 15;
            IMinigameController.instance.streakBonusText.text = "+" + streakBonus.ToString();
        }



        //Calc Net Score
        int calculatedScore = pointValue + streakBonus;

        totalScores[currentPlayer] += calculatedScore;
        individualScore += calculatedScore;
        IMinigameController.instance.scoreText.text = individualScore.ToString();


        //Increase Streak
        streak++;
    }

    public void StartSession()
    {
        companionScreen.NextScreen();
        UpdateAttributes();
        mainScreen.FirstTimeMinigameTransition();

        StartCoroutine(IStartOverallTimer());
    }

    bool timerEnded = false;

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

        timerEnded = true;

        //EndGame();
    }

    public void BeginNewMinigame()
    {

        minigameScores[currentPlayer] = individualScore;

        SceneManager.UnloadSceneAsync(minigameInfos[currentGamemode].sceneName);

        if(currentGamemode >= 3)
        {
            if (timerEnded)
            {
                EndGame();
                return;
            }

            currentPlayer = 0;
            currentGamemode = 0;
            UpdateAttributes();
            mainScreen.EndOfMinigame();
        }
        else if(currentPlayer >= playerCount-1)
        {
            if(timerEnded)
            {
                EndGame();
                return;
            }

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

    public void UpdateAttributes()
    {
        //companionScreen.UpdateMinigameScreen();
        mainScreen.UpdateAttributes();
        OnUpdateAttributes?.Invoke();
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
        mainScreen.EndGame();
        print("endgame");
    }


}
