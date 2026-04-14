using System.Collections;
using UnityEngine;

public class StaticTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;


    [SerializeField]
    GameObject hitParticleEffect;

    bool checkDelete = true;

    void OnEnable()
    {
        SmashTheWallMinigameController.OnMinigameStart += MinigameStart;
    }

    void OnDisable()
    {
        SmashTheWallMinigameController.OnMinigameStart -= MinigameStart;
    }
    public bool canHit = true;

    void MinigameStart()
    {
        canHit = true;
    }

    private void Awake()
    {
        GetComponent<Collider>().enabled = true;
    }





    public void OnHit(Vector3 hitPoint)
    {
        if (!canHit) return;
        Instantiate(hitParticleEffect, new Vector3(hitPoint.x, hitPoint.y, transform.position.z), Quaternion.identity);
        if (GameManager.instance != null) GameManager.instance.UpdateScore(pointValue);
    }



}
