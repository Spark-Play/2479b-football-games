using System.Collections;
using UnityEngine;

public class SplittingTarget : MonoBehaviour, ITargetHandler
{
    [field: SerializeField]
    public int pointValue { get; set; } = 10;

    [SerializeField]
    GameObject model;

    [SerializeField]
    GameObject[] targetVariants;

    bool checkDelete = true;

    private void Awake()
    {
        safe = false;
        GetComponent<Collider>().enabled = true;
        StartCoroutine(ToggleCheckDelete());
    }

    IEnumerator ToggleCheckDelete()
    {
        yield return new WaitForSeconds(0.02f);
        safe = true;
        model.SetActive(true);
    }

    public void ResolveDelete(GameObject otherTarget)
    {
        if(otherTarget.GetComponent<SplittingTarget>().safe)
        {
            print("safe");
            Destroy(gameObject);
        }
        else
        {
            safe = true;
            Destroy(otherTarget);
        }
    }

    [SerializeField]
    public bool safe = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Target")
        {
            if(!safe) Destroy(gameObject);
                //other.GetComponent<SplittingTarget>().ResolveDelete(gameObject);
                //Destroy(gameObject);
        }

        if(other.tag == "OOB")
        {
            Destroy(gameObject);
        }
    }

    public void OnHit(Vector3 hitPoint)
    {
        if(GameManager.instance != null) GameManager.instance.UpdateScore(pointValue);
        StartCoroutine(SpawnNewTargets());
    }


    IEnumerator SpawnNewTargets()
    {
        GetComponent<Collider>().enabled = false;
        model.SetActive(false);

        for (int i = 0; i < 22; i++)
        {
            yield return new WaitForSeconds(0.02f);

            

            GameObject targetObject = Instantiate(targetVariants[Random.Range(0,targetVariants.Length)], transform.parent);
            targetObject.GetComponent<SplittingTarget>().safe = false;
            targetObject.transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-2.1f,2.1f), transform.localPosition.y + Random.Range(-1.1f, 1.1f), 0f);
        }

        Destroy(gameObject);
    }

}
