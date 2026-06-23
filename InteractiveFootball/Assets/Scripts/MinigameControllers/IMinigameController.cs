using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class IMinigameController : MonoBehaviour
{
    public static IMinigameController instance;

    public float countdownDelay = 0;

    [HideInInspector]
    public float secondSpeed = 1;

    [SerializeField]
    TMP_Text timerText;
    [SerializeField]
    TMP_Text countdownText;
    [SerializeField]
    public TMP_Text scoreText;
    [SerializeField]
    public TMP_Text streakBonusText;

    [SerializeField]
    GameObject[] hitEffects;

    [SerializeField]
    Volume defaultVolumeProfile;

    public static event Action OnMinigameStart;

    public bool canShoot = false;



    public Cloth goalNet;


    [SerializeField]
    public Animator hudAnim;


    public void AttachToNet(SphereCollider collider)
    {
        // 1. Get the current list of sphere pairs
        ClothSphereColliderPair[] currentPairs = goalNet.sphereColliders;

        // 2. Create a new array with one extra slot
        ClothSphereColliderPair[] newPairs = new ClothSphereColliderPair[currentPairs.Length + 1];

        // 3. Copy the old pairs to the new array
        for (int i = 0; i < currentPairs.Length; i++)
        {
            newPairs[i] = currentPairs[i];
        }

        // 4. Create the new pair and add it to the last slot
        ClothSphereColliderPair myPair = new ClothSphereColliderPair();
        myPair.first = collider; // You can also set .second for a conical collision
        newPairs[newPairs.Length - 1] = myPair;

        // 5. Reassign the array back to the cloth component
        goalNet.sphereColliders = newPairs;
    }


    public void Start()
    {
        instance = this;

        if(AudioManager.instance != null) StartCoroutine(AudioManager.instance.FadeSound(ambience, true));

        defaultVolumeProfile.profile.TryGet(out colorAdjustments);

        //MinigameIntro();

        if (GameManager.instance == null) MinigameIntro();

        StartCoroutine(TransitionLights(Color.black, Color.white, 1.5f));
    }

    private ColorAdjustments colorAdjustments;

    IEnumerator TransitionLights(Color startColor, Color endColor, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        float duration = 3f;
        float elapsed = 0;



        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            colorAdjustments.colorFilter.value = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }
        colorAdjustments.colorFilter.value = endColor;
    }


    public void MinigameIntro()
    {
        GetReadyCountdown();
    }

    public virtual void StartMinigame()
    {
        GetReadyCountdown();
    }


    IEnumerator IStartTimer()
    {
        int countdownLength = 60;

#if UNITY_EDITOR 
       if(GameManager.instance != null)  countdownLength = 15;
#endif



        for (int i = countdownLength; i >= 0; i--)
        {
            int minutes = i / 60;
            int seconds = i % 60;

            timerText.text = $"{minutes:00}:{seconds:00}";
            yield return new WaitForSeconds(secondSpeed);
        }

        canShoot = false;

        if(AudioManager.instance != null) AudioManager.instance.PlaySound(2);

        hudAnim.Play("OutroHUD");

        //Disable HUD
        //transform.GetChild(0).gameObject.SetActive(false);

        StartCoroutine(TransitionLights(Color.white, Color.grey, 0));
        yield return new WaitForSeconds(3f);


        EndMinigame();

    }


    public void EndMinigame()
    {
        StartCoroutine(AudioManager.instance.FadeSound(ambience, false));
        GameManager.instance.BeginNewMinigame();
    }

    public void GetReadyCountdown()
    {
        StartCoroutine(IGetReadyCountdown(3));
    }

    IEnumerator IGetReadyCountdown(int countdownLength)
    {

        //yield return new WaitForSeconds(countdownDelay);

        for (int i = countdownLength; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }



        countdownText.text = "GO!";

        if (AudioManager.instance != null) AudioManager.instance.PlaySound(1);

        yield return new WaitForSeconds(1f);

        countdownText.text = "";



        hudAnim.Play("IntroHUD");

        //timerText.gameObject.SetActive(true);
        //streakBonusText.transform.parent.gameObject.SetActive(true);
        //scoreText.transform.parent.gameObject.SetActive(true);

        canShoot = true;
        OnMinigameStart.Invoke();


        StartCoroutine(IStartTimer());
    }
    


    [SerializeField]
    Camera camera;



    private void OnEnable()
    {
        if (GameManager.instance != null) GameManager.instance.tcpJsonXYClient.PositionReceived += HandlePosition;
    }

    private void OnDisable()
    {
        if (GameManager.instance != null) GameManager.instance.tcpJsonXYClient.PositionReceived -= HandlePosition;
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

    public virtual void ShotTaken()
    {

    }


    public bool hitAnyTarget = false;
    int targetsHit = 0;

    [SerializeField]
    float raycastRadius = 0.5f;

    public void ShootUsingCoordinates(Vector2 screenCoordinates)
    {
        if(!canShoot) return;
        // 1. Create a ray from the camera to the mouse position
        Ray ray = camera.ScreenPointToRay(screenCoordinates);

        print("shoot");

        ShotTaken();

        RaycastHit[] hits = Physics.SphereCastAll(ray, raycastRadius, 1000f);

        hits = hits.OrderBy(h => h.distance).ToArray();

        hitAnyTarget = false;
        targetsHit = 0;

        foreach (RaycastHit hit in hits)
        {
            // 3. Check for a specific tag or component

            if(hit.collider.CompareTag("SpecificHitbox"))
            {
                hit.collider.GetComponentInParent<ITargetHandler>()?.OnHit(hit.point);
            }

            if (hitAnyTarget == false || targetsHit < 2)
            {
                

                if ((hit.collider.CompareTag("Target") || hit.collider.CompareTag("Debris")))
                {
                    //Debug.Log("Hit the specific object!");

                    // Option A: Classic Unity Message
                    //hit.collider.gameObject.SendMessage("OnObjectClicked", SendMessageOptions.DontRequireReceiver);


                    // Option B: Better practice (Interface or Component call)
                    hit.collider.GetComponentInParent<ITargetHandler>()?.OnHit(hit.point);

                    hitAnyTarget = true;
                    targetsHit++;

                    print("hit target");
                }
                else
                {
                    if (GameManager.instance != null) GameManager.instance.ResetScoreStreak();
                    Instantiate(hitEffects[0], new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
                }

            }

            if (hit.collider.CompareTag("BrickArea"))
            {
                hit.collider.GetComponentInParent<WallTarget>()?.OnComplexHit(hit.point, !hitAnyTarget);
            }
            else if (hit.collider.CompareTag("NetArea"))
            {
                hit.collider.GetComponentInParent<WallTarget>()?.OnComplexHit(hit.point, !hitAnyTarget);
            }
        }

        if(!hitAnyTarget)
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

    [SerializeField] AudioSource ambience;
    

}
