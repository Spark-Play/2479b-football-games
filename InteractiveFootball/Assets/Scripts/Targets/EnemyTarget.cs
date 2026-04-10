using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    public GameObject debugPoint;

    Rigidbody rb;

    bool hit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (hit) return;

        // Calculate the step based on speed and physics time
        Vector3 currentPos = rb.position;
        Vector3 targetPos = new Vector3(0, 0, -7);

        // Move slowly toward the target
        Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, 1 * Time.fixedDeltaTime);

        rb.MovePosition(newPos);
    }

    public void OnHit(Vector3 hitPoint)
    {
        hit = true;

        //Instantiate(debugPoint, hitPoint, Quaternion.identity);
        //print(hitPoint);

        Vector3 forceDirection = transform.forward;

        rb.freezeRotation = false;
        rb.AddForceAtPosition(forceDirection*5, hitPoint, ForceMode.Impulse);

        if (GameManager.instance != null) GameManager.instance.UpdateScore(pointValue);
        StartCoroutine(DestroySequence());
    }
    IEnumerator DestroySequence()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }



}
