using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;


    [SerializeField]
    GameObject hitParticleEffect;

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
        if (GameManager.instance != null) GameManager.instance.UpdateScore(pointValue);
        StartCoroutine(HideSequence());
    }
    IEnumerator HideSequence()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
