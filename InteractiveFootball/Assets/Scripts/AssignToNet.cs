using UnityEngine;

public class AssignToNet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        IMinigameController.instance.AttachToNet(GetComponent<SphereCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
