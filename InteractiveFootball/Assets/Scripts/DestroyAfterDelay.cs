using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{

    public float delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DestorySelf", delay);
    }

    void DestorySelf()
    {
        Destroy(gameObject);
    }
}
