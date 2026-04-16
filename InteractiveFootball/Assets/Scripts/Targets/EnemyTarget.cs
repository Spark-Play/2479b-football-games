using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    public GameObject debugPoint;

    [SerializeField]
    GameObject hitParticleEffect;

    [SerializeField]
    Material redMat;

    Rigidbody rb;

    bool hit = true;

    MeshRenderer mr;


    void OnEnable()
    {
        RushHourMinigameController.OnMinigameStart += MinigameStart;
    }

    void OnDisable()
    {
        RushHourMinigameController.OnMinigameStart -= MinigameStart;
    }

    void MinigameStart()
    {
        hit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OOB")
        {
            hit = true;
            StartCoroutine(DestroySequence());
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {

        if (hit) return;

        // Calculate the step based on speed and physics time
        Vector3 currentPos = rb.position;
        Vector3 targetPos = new Vector3(0, 0, -7);

        // Move slowly toward the target
        Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, 0.6f * Time.fixedDeltaTime);

        rb.MovePosition(newPos);
    }

    public void OnHit(Vector3 hitPoint)
    {
        hit = true;




        Instantiate(hitParticleEffect, hitPoint, Quaternion.identity);
        //print(hitPoint);

        Vector3 forceDirection = transform.forward;

        rb.freezeRotation = false;
        rb.AddForceAtPosition(forceDirection*5, hitPoint, ForceMode.Impulse);

        if (GameManager.instance != null) GameManager.instance.UpdateScore(pointValue);
        StartCoroutine(DestroySequence());
    }
    IEnumerator DestroySequence()
    {
        yield return new WaitForSeconds(1f);
        mr.enabled = false;
        mr.material = redMat;
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


        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }



}
