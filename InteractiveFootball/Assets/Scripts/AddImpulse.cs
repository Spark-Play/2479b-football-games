using UnityEngine;

public class AddImpulse : MonoBehaviour
{
    [SerializeField]
    float power = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * power);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
