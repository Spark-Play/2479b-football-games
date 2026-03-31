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
    TMP_Text instructionsTitle;
    [SerializeField]
    TMP_Text instructionsObjective;
    [SerializeField]
    TMP_Text instructionsFirstUp;

    [Header("TitleCard")]
    [SerializeField]
    TMP_Text titleCardTitle;
    [SerializeField]
    TMP_Text titleCardSubtitle;
    [SerializeField]
    Image titleCardImage;


    [Header("GetReady")]
    [SerializeField]
    TMP_Text getReadySubtitle;
    [SerializeField]
    TMP_Text getReadyTotalScore;

    private MinigameInfo currentMinigameInfo;

    public void UpdateAttributes()
    {
        currentMinigameInfo = GameManager.instance.minigameInfos[GameManager.instance.currentGamemode];

        instructionsTitle.text = currentMinigameInfo.name;
        instructionsObjective.text = currentMinigameInfo.description;
        instructionsFirstUp.text = "First Up: " + GameManager.instance.playerNames[GameManager.instance.currentPlayer];

        titleCardTitle.text = currentMinigameInfo.name;
        titleCardSubtitle.text = currentMinigameInfo.subtitle;
        titleCardImage.sprite = currentMinigameInfo.logo;

        getReadySubtitle.text =  $"{GameManager.instance.playerNames[GameManager.instance.currentPlayer]} | {currentMinigameInfo.name}";
        getReadyTotalScore.text = $"Total Score: {GameManager.instance.totalScores[GameManager.instance.currentPlayer]}";

    }


    int currentScreen = 0;

    public void NextScreen()
    {
        mainScreens[currentScreen].SetActive(false);
        currentScreen++;
        mainScreens[currentScreen].SetActive(true);
    }

    public void MinigameTransition()
    {
        StartCoroutine(IMinigameTransition());
    }

    IEnumerator IMinigameTransition()
    {
        yield return new WaitForSeconds(4f);

        NextScreen();
        yield return new WaitForSeconds(5f);

        NextScreen();
        yield return new WaitForSeconds(4f);

        NextScreen(); 
        yield return new WaitForSeconds(4f);

        NextScreen();
    }

}
