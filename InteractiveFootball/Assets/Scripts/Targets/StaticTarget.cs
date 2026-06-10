using System.Collections;
using UnityEngine;

public class StaticTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    [field: SerializeField]
    public GameObject scorePopup { get; set; }

    [SerializeField]
    GameObject hitParticleEffect;

    bool checkDelete = true;

    public bool cancelStreak = false;

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


        if (cancelStreak == false)
        {

            GameObject scorePopupGameobject = Instantiate(scorePopup, new Vector3(hitPoint.x, hitPoint.y, transform.position.z - 0.5f), Quaternion.identity);
            if (GameManager.instance != null) scorePopupGameobject.GetComponent<SetCustomFields>().SetTextValue("+" + GameManager.instance.UpdateScore(pointValue));


        }
        else
        {

            GameObject scorePopupGameobject = Instantiate(scorePopup, new Vector3(hitPoint.x, hitPoint.y, transform.position.z - 0.5f), Quaternion.identity);
            if (GameManager.instance != null) scorePopupGameobject.GetComponent<SetCustomFields>().SetTextValue("+" + GameManager.instance.UpdateScoreCancelStreak(pointValue));


        }
    }



}
