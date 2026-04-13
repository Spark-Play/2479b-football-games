using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class IMinigameController : MonoBehaviour
{
    public static IMinigameController instance;

    public float countdownDelay = 0;

    [SerializeField]
    TMP_Text timerText;
    [SerializeField]
    TMP_Text countdownText;
    [SerializeField]
    public TMP_Text scoreText;
    [SerializeField]
    public TMP_Text streakBonusText;

    public static event Action OnMinigameStart;

    void Awake()
    {
        instance = this;

        MinigameIntro();
    }


    public void MinigameIntro()
    {
        GetReadyCountdown();
    }

    public void StartMinigame()
    {
        OnMinigameStart.Invoke();
        StartCoroutine(IStartTimer(60));
    }


    IEnumerator IStartTimer(int countdownLength)
    {
        for (int i = countdownLength; i >= 0; i--)
        {
            int minutes = i / 60;
            int seconds = i % 60;

            timerText.text = $"{minutes:00}:{seconds:00}";
            yield return new WaitForSeconds(1f);
        }

        EndMinigame();

    }


    public void EndMinigame()
    {
        GameManager.instance.BeginNewMinigame();
    }

    public void GetReadyCountdown()
    {
        StartCoroutine(IGetReadyCountdown(3));
    }

    IEnumerator IGetReadyCountdown(int countdownLength)
    {

        yield return new WaitForSeconds(countdownDelay);

        for (int i = countdownLength; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";


        yield return new WaitForSeconds(1f);

        countdownText.text = "";

        StartMinigame();
    }



    [SerializeField]
    Camera camera;



    private void OnEnable()
    {
        GameManager.instance.tcpJsonXYClient.PositionReceived += HandlePosition;
    }

    private void OnDisable()
    {
        GameManager.instance.tcpJsonXYClient.PositionReceived -= HandlePosition;
    }

    private void HandlePosition(XYMessage message)
    {
        Debug.Log($"X: {message.x}, Y: {message.y}");

        float xPct = message.x;
        float yPct = message.y;

        // Convert 0-1 range to actual pixel coordinates
        float pixelX = xPct * Screen.width;
        float pixelY = yPct * Screen.height;

        Vector3 screenPoint = new Vector3(pixelX, pixelY, 0);

        ShootUsingCoordinates(screenPoint);
    }

    public void ShootUsingCoordinates(Vector2 screenCoordinates)
    {
        // 1. Create a ray from the camera to the mouse position
        Ray ray = camera.ScreenPointToRay(screenCoordinates);
        RaycastHit hit;

        // 2. Shoot the ray and see if it hits a collider
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            // 3. Check for a specific tag or component
            if (hit.collider.CompareTag("Target"))
            {
                //Debug.Log("Hit the specific object!");

                // Option A: Classic Unity Message
                //hit.collider.gameObject.SendMessage("OnObjectClicked", SendMessageOptions.DontRequireReceiver);


                // Option B: Better practice (Interface or Component call)
                hit.collider.GetComponentInParent<ITargetHandler>()?.OnHit(hit.point);
            }
            else
            {
                if(GameManager.instance != null) GameManager.instance.ResetScoreStreak();
            }
        }
        else
        {
            if (GameManager.instance != null) GameManager.instance.ResetScoreStreak();
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
