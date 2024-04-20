using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class EffectAudioSource : MonoBehaviour
{
    private AudioSource audioSource = null;
    public AudioSource AudioSource
    {
        get { return audioSource; }
    }

    private UnityAction<EffectAudioSource> onPlayEnd = (EffectAudioSource effectAudioSource) => { };
    public UnityAction<EffectAudioSource> OnPlayEnd
    {
        get { return onPlayEnd; }
        set { onPlayEnd = value; }
    }

    private bool timerStart = false;
    private float playTimer = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        CheckPlayTimer();
    }

    private void CheckPlayTimer()
    {
        if (timerStart)
        {
            playTimer += Time.deltaTime;

            if (playTimer >= audioSource.clip.length)
            {
                timerStart = false;

                onPlayEnd.Invoke(this);
            }
        }
    }

    public void SetClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void Play()
    {
        playTimer = 0f;
        timerStart = true;

        audioSource.Play();
    }
}
