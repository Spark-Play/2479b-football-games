using System;
using TMPro;
using UnityEngine;

public class AutoUpdatedText : MonoBehaviour
{

    public string textFormat;

    public enum TextType { playerName, minigameName, totalScore };

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
            text = GameManager.instance.playerNames[GameManager.instance.currentPlayer];
        }
        else if(textType == TextType.minigameName)
        {
            text = GameManager.instance.minigameInfos[GameManager.instance.currentGamemode].name;
        }
        else if (textType == TextType.totalScore)
        {
            text = GameManager.instance.totalScores[GameManager.instance.currentPlayer].ToString();
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
