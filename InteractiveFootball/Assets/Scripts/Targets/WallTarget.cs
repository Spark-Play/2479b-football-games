using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WallTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;


    [SerializeField]
    GameObject hitParticleEffect;


    void OnEnable()
    {
        IMinigameController.OnMinigameStart += MinigameStart;
    }

    void OnDisable()
    {
        IMinigameController.OnMinigameStart -= MinigameStart;
    }
    public bool canHit = false;

    void MinigameStart()
    {
        canHit = true;
    }

    public void OnHit(Vector3 hitPoint)
    {
        if (!canHit) return;
        Instantiate(hitParticleEffect, new Vector3(hitPoint.x, hitPoint.y, transform.position.z), Quaternion.identity);
        if (GameManager.instance != null) GameManager.instance.UpdateScoreCancelStreak(pointValue);
    }

}
