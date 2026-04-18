using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DebrisTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;


    [SerializeField]
    GameObject hitParticleEffect;

    public float forceStrength = 5;


    Rigidbody rb;
    public MeshRenderer mr;

    Vector3 initialPosition;
    Quaternion initialRotation;

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
        rb = GetComponent<Rigidbody>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }




    public void OnHit(Vector3 hitPoint)
    {
        if (!canHit) return;
        Instantiate(hitParticleEffect, new Vector3(hitPoint.x, hitPoint.y, transform.position.z), Quaternion.identity);
        if (GameManager.instance != null) GameManager.instance.UpdateScoreCancelStreak(pointValue);

        Vector3 forceDirection = transform.forward;

        rb.AddForceAtPosition(forceDirection * forceStrength, hitPoint, ForceMode.Impulse);
        StartCoroutine(DestroySequence());
    }


    IEnumerator DestroySequence()
    {
        yield return new WaitForSeconds(1f);
        mr.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mr.enabled = true;
        yield return new WaitForSeconds(0.1f);
        mr.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mr.enabled = true;
        yield return new WaitForSeconds(0.1f);
        mr.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mr.enabled = true;
        yield return new WaitForSeconds(0.06f);
        mr.enabled = false;
        yield return new WaitForSeconds(0.06f);
        mr.enabled = true;
        yield return new WaitForSeconds(0.06f);
        mr.enabled = false;
        yield return new WaitForSeconds(0.03f);
        mr.enabled = true;
        yield return new WaitForSeconds(0.03f);
        mr.enabled = false;

        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;

        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(0.5f);
        mr.enabled = true;
    }



}
