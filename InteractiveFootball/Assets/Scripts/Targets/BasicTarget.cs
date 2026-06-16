using Rive.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class BasicTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    [field: SerializeField]
    public GameObject scorePopup { get; set; }

    [SerializeField]
    GameObject hitParticleEffect;

    [SerializeField]
    RiveWidget targetRive;


    [SerializeField] MeshRenderer riveMesh;

    Material riveMaterial;

    bool checkDelete = true;

    public bool firstTarget = false;


    [field: SerializeField]
    public AudioSource hitSound { get; set; }

    private void Start()
    {
        if (firstTarget)
        {
            Invoke("ResetRive", 0.01f);
            firstTarget = false;
        }
    }

   void ResetRive()
    {
        targetRive?.StateMachine?.ViewModelInstance?.GetTriggerProperty("reset")?.Trigger();
    }

    void OnEnable()
    {


        StartCoroutine(DrawRive());

        IMinigameController.OnMinigameStart += MinigameStart;
    }

    void OnDisable()
    {
        IMinigameController.OnMinigameStart -= MinigameStart;
    }
    public bool canHit = true;

    void MinigameStart()
    {
        canHit = true;

        StartCoroutine(DrawRive());
    }

    IEnumerator DrawRive()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));

        targetRive?.StateMachine?.ViewModelInstance?.GetTriggerProperty("draw")?.Trigger();
    }
        

    private void Awake()
    {
        GetComponent<Collider>().enabled = true;

        riveMaterial = riveMesh.material;
    }





    public void OnHit(Vector3 hitPoint)
    {
        if (!canHit) return;
        canHit = false;

        hitSound.Play();

        if (GameManager.instance != null) Instantiate(hitParticleEffect, new Vector3(hitPoint.x, hitPoint.y, transform.position.z), Quaternion.identity);

        GameObject scorePopupGameobject = Instantiate(scorePopup, new Vector3(hitPoint.x, hitPoint.y, transform.position.z - 0.5f), Quaternion.identity);
        if (GameManager.instance != null) scorePopupGameobject.GetComponent<SetCustomFields>().SetTextValue("+" + GameManager.instance.UpdateScore(pointValue));

        HideSequenceCoroutine();
    }
    public void HideSequenceCoroutine()
    {
        if(gameObject.activeSelf) StartCoroutine(HideSequence());
    }

    IEnumerator HideSequence()
    {


        yield return StartCoroutine(TransitionMaterialColor(Color.green, Color.clear, 0.5f));


        targetRive?.StateMachine?.ViewModelInstance?.GetTriggerProperty("reset")?.Trigger();

        yield return new WaitForSeconds(0.05f);


        riveMaterial.color = Color.white;

        canHit = true;

        //gameObject.SetActive(false);


        //yield return StartCoroutine(FadeTransition(canvasGroup, true));

    }



    IEnumerator TransitionMaterialColor(Color startColor, Color endColor, float startDelay)
    {

        yield return new WaitForSeconds(startDelay);

        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            riveMaterial.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }
        riveMaterial.color = endColor;
    }

}
