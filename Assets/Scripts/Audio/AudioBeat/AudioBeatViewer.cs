using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBeatViewer : MonoBehaviour
{
    [SerializeField]
    private AudioBeatMaker audioBeatMaker = null;
    [SerializeField]
    private AudioSource audioSource = null;

    [SerializeField]
    private RectTransform markObject = null;
    [SerializeField]
    private RectTransform markBarObject = null;

    [SerializeField]
    private float xLengthPerSec = 1f;

    private float audioClipLength = 0f;
    private float currentPlayTime = 0f;

    private bool checkMark = false;

    private void OnEnable()
    {
        audioBeatMaker.onPlayMusic += OnPlayMusic;
        audioBeatMaker.whenPlayingMusic += WhenPlayingMusic;
        audioBeatMaker.onEndMusic += OnEndMusic;
        audioBeatMaker.onBeatMusic += OnBeatMusic;
        audioBeatMaker.onSetClip += OnSetClip;
    }

    private void OnDisable()
    {
        audioBeatMaker.onPlayMusic -= OnPlayMusic;
        audioBeatMaker.whenPlayingMusic -= WhenPlayingMusic;
        audioBeatMaker.onEndMusic -= OnEndMusic;
        audioBeatMaker.onBeatMusic -= OnBeatMusic;
        audioBeatMaker.onSetClip -= OnSetClip;
    }

    private void Start()
    {
        var sizeDelta = markObject.sizeDelta;
        sizeDelta.x = xLengthPerSec;
        markObject.sizeDelta = sizeDelta;
    }

    void Update()
    {
        SetMarkBarPosition();
    }

    private void OnPlayMusic()
    {
        // start Mark

        checkMark = true;
    }
    private void WhenPlayingMusic(float time)
    {
        // Check Current Playtime

        currentPlayTime = time;
    }
    private void OnEndMusic()
    {
        // end Mark

        checkMark = false;
    }
    private void OnBeatMusic()
    {
        // Init BeatObject


    }
    private void OnSetClip(AudioClip clip)
    {
        // set clip length

        audioClipLength = clip.length;

        var size = markBarObject.sizeDelta;

        size.x = clip.length;

        markBarObject.sizeDelta = size;
    }

    private void SetMarkBarPosition()
    {
        if (checkMark)
        {
            Vector3 curPos = markObject.localPosition;

            curPos.x += xLengthPerSec * Time.deltaTime;

            markObject.localPosition = curPos;
        }
    }
}
