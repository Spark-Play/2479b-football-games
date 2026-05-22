using Rive.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    [field: SerializeField]
    public GameObject scorePopup { get; set; }

    [SerializeField]
    GameObject hitParticleEffect;

    [SerializeField]
    RiveWidget targetRive;

    bool checkDelete = true;

    void OnEnable()
    {
        IMinigameController.OnMinigameStart += MinigameStart;
    }

    void OnDisable()
    {
        IMinigameController.OnMinigameStart -= MinigameStart;
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

        GameObject scorePopupGameobject = Instantiate(scorePopup, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), Quaternion.identity);
        if (GameManager.instance != null) scorePopupGameobject.GetComponent<SetCustomFields>().SetTextValue("+" + GameManager.instance.UpdateScore(pointValue));



        targetRive.StateMachine.ViewModelInstance.GetTriggerProperty("hitbox1Click").Trigger();

        StartCoroutine(HideSequence());
    }
    IEnumerator HideSequence()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
