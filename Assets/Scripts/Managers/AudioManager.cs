using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeDatas
{
    public float masterVolume = 0f;
    public float mainBGMVolume = 0f;
    public float effectVolume = 0f;
}
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource mainBGMAudioSource = null;
    private Queue<EffectAudioSource> effectAudioSource = new();

    private VolumeDatas volumeData = new();
    public VolumeDatas VolumeData
    {
        get { return volumeData; }
    }

    protected override void Awake()
    {
        base.Awake();

        if(mainBGMAudioSource == null)
        {
            mainBGMAudioSource = GetComponent<AudioSource>();
        }
    }

    public AudioSource GetMainBGMAudioSource()
    {
        if(mainBGMAudioSource == null)
        {
            mainBGMAudioSource = GetComponent<AudioSource>();
        }

        return mainBGMAudioSource;
    }

    public void SetMainBGMClip(AudioClip clip)
    {
        mainBGMAudioSource.clip = clip;
    }
    public void PlayMainBGM()
    {
        mainBGMAudioSource.Play();
    }

    public void PlayEffect(AudioClip clip)
    {
        EffectAudioSource audioSource = DequeueEffectAudioSource();
        if (audioSource == null)
        {
            GameObject audiGameObject = new GameObject("EffectAudioSource");
            audioSource = audiGameObject.AddComponent<EffectAudioSource>();
        }

        audioSource.OnPlayEnd += EnqueueEffectAudioSource;

        audioSource.AudioSource.clip = clip;
        audioSource.Play();
    }

    private void EnqueueEffectAudioSource(EffectAudioSource audioSource)
    {
        audioSource.AudioSource.volume = volumeData.masterVolume * volumeData.effectVolume;
        effectAudioSource.Enqueue(audioSource);
    }

    private EffectAudioSource DequeueEffectAudioSource()
    {
        return effectAudioSource.Dequeue();
    }
}
