using UnityEngine;

public class ShotHandler : MonoBehaviour
{
    


    public void ShootUsingCoordinates(Vector2 screenCoordinates)
    {
        // 1. Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(screenCoordinates);
        RaycastHit hit;

        // 2. Shoot the ray and see if it hits a collider
        if (Physics.Raycast(ray, out hit))
        {
            // 3. Check for a specific tag or component
            if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("Hit the specific object!");

                // Option A: Classic Unity Message
                //hit.collider.gameObject.SendMessage("OnObjectClicked", SendMessageOptions.DontRequireReceiver);

                // Option B: Better practice (Interface or Component call)
                hit.collider.GetComponent<ITargetHandler>()?.OnHit();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button was pressed
        if (Input.GetMouseButtonDown(0))
        {
            ShootUsingCoordinates(Input.mousePosition);
        }
    }
}
