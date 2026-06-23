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


    public IEnumerator FadeSound(int id, bool fadeIn)
    {
        AudioSource audioSource = audioSources[id];

        if (fadeIn)
        {
            float endVolume = audioSource.volume;

            audioSource.volume = 0;

            audioSource.Play();

            // Loop until the duration has passed
            while (audioSource.volume < endVolume)
            {
                // Increase volume linearly over time
                audioSource.volume += endVolume * Time.deltaTime / 3;

                yield return null; // Wait for the next frame
            }

            audioSource.volume = endVolume;
        }
        else
        {
            float startVolume = audioSource.volume;
            // Loop until the duration has passed
            while (audioSource.volume > 0)
            {
                // Reduce volume linearly over time
                audioSource.volume -= startVolume * Time.deltaTime / 3;

                yield return null; // Wait for the next frame
            }


            audioSource.Stop();
            audioSource.volume = startVolume; // Reset volume for next time it plays
        }
    }

    public IEnumerator FadeSound(AudioSource audioSource, bool fadeIn)
    {

        if (fadeIn)
        {
            float endVolume = audioSource.volume;

            audioSource.volume = 0;

            audioSource.Play();

            // Loop until the duration has passed
            while (audioSource.volume < endVolume)
            {
                // Increase volume linearly over time
                audioSource.volume += Time.deltaTime / 10;

                yield return null; // Wait for the next frame
            }

            audioSource.volume = endVolume;
        }
        else
        {
            float startVolume = audioSource.volume;
            // Loop until the duration has passed
            while (audioSource.volume > 0)
            {
                // Reduce volume linearly over time
                audioSource.volume -= startVolume * Time.deltaTime / 3;

                yield return null; // Wait for the next frame
            }


            audioSource.Stop();
            audioSource.volume = startVolume; // Reset volume for next time it plays
        }
    }
}
