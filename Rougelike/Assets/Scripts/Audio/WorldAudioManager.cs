using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAudioManager : MonoBehaviour
{
    public AudioClip defaulAudioClip;

    private AudioSource track1, track2;
    private bool isPlayingTrack1;

    public static WorldAudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    private void Start()
    {
        track1 = gameObject.AddComponent<AudioSource>();
        track2 = gameObject.AddComponent<AudioSource>();
        track1.loop = true;
        track2.loop = true;
        isPlayingTrack1 = true;

        SwapTrack(defaulAudioClip);
    }

    public void SwapTrack(AudioClip newClip)
    {
        StopAllCoroutines();

        StartCoroutine(FadeTrack(newClip));

        isPlayingTrack1 = !isPlayingTrack1;
    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {

        float timeToFade = 1.25f;
        float timeElapsed = 0;


        if (isPlayingTrack1)
        {
            track2.clip = newClip;
            track2.Play();

            while(timeElapsed < timeToFade)
            {
                track2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track1.Stop();
        }
        else
        {
            track1.clip = newClip;
            track1.Play();
            while (timeElapsed < timeToFade)
            {
                track1.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track2.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            track2.Stop();
        }
    }
    public void ReturnOnDefault()
    {
        SwapTrack(defaulAudioClip);
    }
}
