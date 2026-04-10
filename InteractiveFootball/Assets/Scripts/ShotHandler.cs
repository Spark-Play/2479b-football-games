using UnityEngine;

public class ShotHandler : MonoBehaviour
{

    //[SerializeField]
    //Camera camera;

    //public void ShootUsingCoordinates(Vector2 screenCoordinates)
    //{
    //    // 1. Create a ray from the camera to the mouse position
    //    Ray ray = camera.ScreenPointToRay(screenCoordinates);
    //    RaycastHit hit;

    //    print("SHOOT");
    //    // 2. Shoot the ray and see if it hits a collider
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        // 3. Check for a specific tag or component
    //        if (hit.collider.CompareTag("Target"))
    //        {
    //            //Debug.Log("Hit the specific object!");

    //            // Option A: Classic Unity Message
    //            //hit.collider.gameObject.SendMessage("OnObjectClicked", SendMessageOptions.DontRequireReceiver);

    //            // Option B: Better practice (Interface or Component call)
    //            hit.collider.GetComponentInParent<ITargetHandler>()?.OnHit();
    //        }
    //        else
    //        {
    //            print("RESERT");
    //            GameManager.instance.ResetScoreStreak();
    //        }
    //    }
    //    else
    //    {
    //        print("RESERT");
    //        GameManager.instance.ResetScoreStreak();
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // Check if the left mouse button was pressed
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        ShootUsingCoordinates(Input.mousePosition);
    //    }
    //}
}
