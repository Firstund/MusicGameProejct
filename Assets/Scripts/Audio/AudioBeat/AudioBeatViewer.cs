using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBeatViewer : MonoBehaviour
{
    [SerializeField]
    private AudioBeatMaker audioBeatMaker = null;
    [SerializeField]
    private MarkBar markBar = null;
    [SerializeField]
    private GameObject markPoint = null;
    private List<GameObject> markPointList = new();

    [SerializeField]
    private AudioSource audioSource = null;

    [SerializeField]
    private RectTransform markObject = null;
    [SerializeField]
    private RectTransform markBarObject = null;

    private Vector3 originMarkPosition = Vector3.zero;

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

        originMarkPosition = markObject.localPosition;
    }

    void Update()
    {
        SetMarkBarPosition();
    }

    private void OnPlayMusic()
    {
        // start Mark
        markObject.localPosition = originMarkPosition;

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
    private void OnBeatMusic(float time)
    {
        // Init BeatObject

        AddMarkPoint(time);
    }
    private void OnSetClip(AudioClip clip)
    {
        // set clip length

        audioClipLength = clip.length;

        var size = markBarObject.sizeDelta;

        size.x = clip.length * xLengthPerSec;

        markBarObject.sizeDelta = size;
    }

    private void AddMarkPoint(float time)
    {
        GameObject markObject = Instantiate(markPoint);
        MarkPoint mark = markObject.GetComponent<MarkPoint>();

        mark.SetMarkTime(markBar.MarkPoints.transform, time, xLengthPerSec);

        markPointList.Add(markObject);
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
