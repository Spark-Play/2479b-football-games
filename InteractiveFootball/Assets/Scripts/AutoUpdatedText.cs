using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoUpdatedText : MonoBehaviour
{

    public int indexOverride = -1;

    public string textFormat;

    public enum TextType { playerName, playerNumber, minigameName, totalScore, individualScore, minigameLogo };

    public TextType textType;

    void OnEnable()
    {
        GameManager.OnUpdateAttributes += UpdateText;
        UpdateText();
    }

    void OnDisable()
    {
        GameManager.OnUpdateAttributes -= UpdateText;
    }


    void UpdateText()
    {
        //print("UPDATED" + gameObject.name);

        string text = "no data found";

        if(textType == TextType.playerName)
        {
            int index = (indexOverride != -1) ? indexOverride : GameManager.instance.currentPlayer;
            text = GameManager.instance.playerNames[index];
        }
        else if(textType == TextType.minigameName)
        {
            int index = (indexOverride != -1) ? indexOverride : GameManager.instance.currentGamemode;
            text = GameManager.instance.minigameInfos[index].name;
        }
        else if (textType == TextType.totalScore)
        {
            int index = (indexOverride != -1) ? indexOverride : GameManager.instance.currentPlayer;
            text = GameManager.instance.totalScores[index].ToString();
        }
        else if (textType == TextType.playerNumber)
        {
            int index = (indexOverride != -1) ? indexOverride : GameManager.instance.currentPlayer+1;
            text = index.ToString();
        }
        else if (textType == TextType.individualScore)
        {
            text = GameManager.instance.individualScore.ToString();
        }

        if(textType == TextType.minigameLogo)
        {
            int index = (indexOverride != -1) ? indexOverride : GameManager.instance.currentGamemode;
            GetComponent<Image>().sprite = GameManager.instance.minigameInfos[index].logo;
        }
        else if (textFormat == "")
        {
            GetComponent<TMP_Text>().text = text;
        }
        else
        {
            GetComponent<TMP_Text>().text = string.Format(textFormat, text);
        }         
    }
}
