using System;
using System.Collections;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{

    public float delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TryGetComponent<RectTransform>(out RectTransform rect))
        {
            StartCoroutine(MoveUILeftForDuration());
        }


        Invoke("DestorySelf", delay);
    }

    void DestorySelf()
    {
        Destroy(gameObject);
    }

    IEnumerator MoveUILeftForDuration()
    {
        yield return new WaitForSeconds(3f);

        float elapsedTime = 0f;

        while (elapsedTime < 2)
        {
            // Calculate how far to move this frame (Speed * Time)
            float moveAmount = 5000 * Time.deltaTime;

            // Subtract from the X axis to move it left
           
            
            GetComponent<RectTransform>().anchoredPosition -= new Vector2(moveAmount, 0f);

            // Track time
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }
    }

}
