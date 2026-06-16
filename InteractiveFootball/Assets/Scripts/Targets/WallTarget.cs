using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WallTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    [field: SerializeField]
    public AudioSource hitSound { get; set; }
    public GameObject scorePopup { get; set; }

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
    public void OnComplexHit(Vector3 hitPoint, bool addScore)
    {
        if (!canHit) return;

        hitSound.Play();
        Instantiate(hitParticleEffect, new Vector3(hitPoint.x, hitPoint.y, transform.position.z), Quaternion.identity);
        if (addScore) if (GameManager.instance != null) GameManager.instance.UpdateScoreCancelStreak(pointValue);
    }

    public void OnHit(Vector3 hitPoint)
    {
        if (!canHit) return;

        hitSound.Play();
        Instantiate(hitParticleEffect, new Vector3(hitPoint.x, hitPoint.y, transform.position.z), Quaternion.identity);
        if (GameManager.instance != null) GameManager.instance.UpdateScoreCancelStreak(pointValue);
    }

}
