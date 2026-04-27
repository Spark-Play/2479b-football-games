using UnityEngine;

public class ChangeToPlayerColour : MonoBehaviour
{

    [SerializeField]
    ParticleSystem[] particles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        foreach (var item in particles)
        {
            item.startColor = GameManager.instance.playerColours[GameManager.instance.currentPlayer];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
