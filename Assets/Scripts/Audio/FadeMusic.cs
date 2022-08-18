using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeMusic
{
    public static IEnumerator StartFade(AudioSource audioSource, float fadeDuration, float targetVolume)
    {
        float currentTime = 0;
        float startingVolume = audioSource.volume;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startingVolume, targetVolume, currentTime / fadeDuration);
            yield return null;
        }

        yield break;
    }
}
