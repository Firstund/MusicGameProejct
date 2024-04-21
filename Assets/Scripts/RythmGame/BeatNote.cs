using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatNote : MonoBehaviour
{
    [SerializeField]
    private BeatNoteType beatNoteType = BeatNoteType.Up;
    public BeatNoteType BeatNoteType
    {
        get { return beatNoteType; }
        set { beatNoteType = value; }
    }

    private float onSpawnAudioSourceTime = 0f;
    public float OnSpawnAudioSourceTime
    {
        get { return onSpawnAudioSourceTime; }
    }

    private float reachToTargetAudioSourceTime = 0f;
    public float ReachToTargetAudioSourceTime
    {
        get { return reachToTargetAudioSourceTime; }
    }

    private float spectrumValue = 0f;
    public float SpectrumValue
    {
        get { return spectrumValue; }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    private bool isReachToTargetTimerStart = false;

    private float reachToTargetTime = 0f;
    public float ReachToTargetTime
    {
        get { return reachToTargetTime;}
    }
    private float reachToTargetTimer = 0f;
    public float ReachToTargetTimer
    {
        get
        {
            return reachToTargetTimer;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        CheckTimers();
    }

    private void CheckTimers()
    {
        if (RythmGameManager.Instance.IsPlayingRythmGame)
        {
            if (isReachToTargetTimerStart)
            {
                reachToTargetTimer += Time.deltaTime;

                if (reachToTargetTimer >= reachToTargetTime)
                {
                    isReachToTargetTimerStart = false;

                    Destroy(gameObject); // 후에 Pooling 추가
                }
            }
        }
    }

    public void Init(BeatNoteSpawnData beatNoteSpawnData)
    {
        onSpawnAudioSourceTime = beatNoteSpawnData.spawnTime;
        reachToTargetAudioSourceTime = beatNoteSpawnData.reachTime;
        spectrumValue = beatNoteSpawnData.spectrumValue;

        reachToTargetTime = reachToTargetAudioSourceTime - onSpawnAudioSourceTime;
        reachToTargetTimer = 0f;

        isReachToTargetTimerStart = true;
    }
}
