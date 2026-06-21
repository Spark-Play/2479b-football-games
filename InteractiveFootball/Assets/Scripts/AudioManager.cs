using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    AudioSource[] audioSources;

    void Awake()
    {
        instance = this;
    }

    public void PlaySound(int id)
    {
        audioSources[id].Play();
    }

    public IEnumerator FadeOutSound(int id)
    {

        AudioSource mySource = audioSources[id];

        float startVolume = mySource.volume;

        // Loop until the duration has passed
        while (mySource.volume > 0)
        {
            // Reduce volume linearly over time
            mySource.volume -= startVolume * Time.deltaTime / 3;

            yield return null; // Wait for the next frame
        }

        mySource.Stop();
        mySource.volume = startVolume; // Reset volume for next time it plays
    }
}
