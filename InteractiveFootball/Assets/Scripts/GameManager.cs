using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int currentPlayer = 0;
    public int currentGamemode = 0;
    public int playerCount = 0;
    public MinigameInfo[] minigameInfos;

    public int[] totalScores;
    public string[] playerNames;
    public float duration = 15;

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

}
