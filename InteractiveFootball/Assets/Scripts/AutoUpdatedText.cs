using System;
using TMPro;
using UnityEngine;

public class AutoUpdatedText : MonoBehaviour
{

    public int indexOverride = -1;

    public string textFormat;

    public enum TextType { playerName, minigameName, totalScore, individualScore };

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
            text = GameManager.instance.minigameInfos[GameManager.instance.currentGamemode].name;
        }
        else if (textType == TextType.totalScore)
        {
            int index = (indexOverride != -1) ? indexOverride : GameManager.instance.currentPlayer;
            text = GameManager.instance.totalScores[GameManager.instance.currentPlayer].ToString();
        }
        else if (textType == TextType.individualScore)
        {
            text = GameManager.instance.individualScore.ToString();
        }


        if (textFormat == "")
        {
            GetComponent<TMP_Text>().text = text;
        }
        else
        {
            GetComponent<TMP_Text>().text = string.Format(textFormat, text);
        }         
    }
}
