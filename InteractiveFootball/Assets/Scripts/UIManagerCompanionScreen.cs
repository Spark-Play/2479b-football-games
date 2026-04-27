using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class UIManagerCompanionScreen : MonoBehaviour
{
    [SerializeField]
    GameObject[] companionScreens;

    [Header("Dynamic Attributes")]
    [Header("Setup")]
    [SerializeField]
    TMP_InputField[] setupPlayerNameInputs;
    [SerializeField]
    TMP_Text setupDuration;
    [SerializeField]
    TMP_Text setupPlayerCount;
    [SerializeField]
    TMP_Text[] setupPlayerNames;

    [Header("Session")]
    [SerializeField]
    TMP_Text sessionCurrentMode;
    [SerializeField]
    TMP_Text sessionPlayer;
    [SerializeField]
    public Slider progressSlider;
    [SerializeField]
    public TMP_Text sessionTimeRemaining;
    [SerializeField]
    TMP_Text sessionWinner;
    [SerializeField]
    TMP_Text sessionTotalScore;

    public void SetGameDuration(int duration)
    {
        GameManager.instance.duration = duration;

        setupDuration.text = duration + " Minutes";
    }

    public void SetPlayerCount(int count)
    {
        GameManager.instance.playerCount = count;

        setupPlayerCount.text = GameManager.instance.playerCount.ToString();

        for (int i = 0; i < setupPlayerNames.Length; i++)
        {
            setupPlayerNameInputs[i].gameObject.SetActive(i < GameManager.instance.playerCount);
            setupPlayerNames[i].gameObject.SetActive(i < GameManager.instance.playerCount); 
        }   
    }
    public void SetPlayerNames()
    {
        for (int i = 0; i < GameManager.instance.playerCount; i++)
        {
            if(setupPlayerNameInputs[i].text !="") GameManager.instance.playerNames[i] = setupPlayerNameInputs[i].text;

            setupPlayerNames[i].text = GameManager.instance.playerNames[i];
            //print(GameManager.instance.playerNames[i]);
        }
    }

    //public void UpdateMinigameScreen()
    //{
    //    sessionCurrentMode.text = GameManager.instance.minigameInfos[GameManager.instance.currentGamemode].name;
    //    sessionPlayer.text = GameManager.instance.playerNames[GameManager.instance.currentPlayer];

    //}


    public void StartSession()
    {
        GameManager.instance.StartSession();
    }



    int currentScreen = 0;

    public void NextScreen()
    {
        companionScreens[currentScreen].SetActive(false);
        currentScreen++;
        companionScreens[currentScreen].SetActive(true);
    }


    public void EndGame(Dictionary<int, int> sortedLeaderboard)
    {
        NextScreen();

        sessionWinner.text = "Winner: " + GameManager.instance.playerNames[sortedLeaderboard.First().Key];
        sessionTotalScore.text = "Total Score: " + sortedLeaderboard.First().Value.ToString();
    }

}
