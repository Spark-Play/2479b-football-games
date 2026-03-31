using System.Collections;
using TMPro;
using UnityEngine;

public class IMinigameController : MonoBehaviour
{
    public static IMinigameController instance;

    [SerializeField]
    TMP_Text countdownText;

    void Awake()
    {
        instance = this;
    }


    public void StartMinigame()
    {
        StartCoroutine(IStartCountdown(10));
    }


    IEnumerator IStartCountdown(int countdownLength)
    {
        for (int i = countdownLength; i >= 0; i--)
        {
            int minutes = i / 60;
            int seconds = i % 60;

            countdownText.text = $"{minutes:00}:{seconds:00}";
            yield return new WaitForSeconds(1f);
        }

        EndMinigame();

    }


    public void EndMinigame()
    {
        GameManager.instance.BeginNewMinigame();
    }

}
