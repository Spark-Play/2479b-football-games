using UnityEngine;

public class IMinigameController : MonoBehaviour
{
    public static IMinigameController instance;

    void Awake()
    {
        instance = this;
    }
}
