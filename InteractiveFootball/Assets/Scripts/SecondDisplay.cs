using UnityEngine;

public class SecondDisplay : MonoBehaviour
{
    void Awake()
    {
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
