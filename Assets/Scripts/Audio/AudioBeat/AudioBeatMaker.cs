using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class BeatDataSaveType
{
    public List<BeatDataType> beatDataSaveTypeList = new();
}
[Serializable]
public class BeatDataType
{
    public int beatIndex = 0;

    public float beatTime = 0f;

    public float spectrumValue = 0f;
}
public class AudioBeatMaker : AudioSyncer
{
    [SerializeField]
    private Slider timeStepSlider = null;
    [SerializeField]
    private Slider sensitivitySlider = null;

    [SerializeField]
    private float maxTimeStep = 10f;
    [SerializeField]
    private float maxSensitivity = 100f;
    [SerializeField]
    private float sensitivity = 0f;

    private List<BeatDataType> beatDataList = new();

    public UnityAction onPlayMusic = () => { };
    public UnityAction<AudioClip> onSetClip = (AudioClip clip) => { };
    public UnityAction<float> whenPlayingMusic = (float time) => { };
    public UnityAction onBeatMusic = () => { };
    public UnityAction onEndMusic = () => { };

    private float audioLength = 0f;
    public float AudioLength
    {
        get
        {
            return audioLength;
        }
    }

    private bool prevPlaying = false;

    protected override void Start()
    {
        base.Start();

        sensitivitySlider.maxValue = maxSensitivity;
        timeStepSlider.maxValue = maxTimeStep;

        sensitivitySlider.value = sensitivity;
        timeStepSlider.value = timeStep;

        if (sensitivity > maxSensitivity)
        {
            sensitivity = maxSensitivity;
        }

        if(audioSource.clip != null)
        {
            onSetClip.Invoke(audioSource.clip);
        }

        //audioSource.
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void OnBeat()
    {
        base.OnBeat();

        if ((maxSensitivity - sensitivity) <= m_audioValue)
        {
            BeatDataType beatDataType = new();

            beatDataType.beatIndex = beatDataList.Count;
            beatDataType.beatTime = audioSource.time;
            beatDataType.spectrumValue = m_audioValue;

            beatDataList.Add(beatDataType);

            Debug.Log("Data Saved! SavedValue: " + m_audioValue + ", SavedTime: " + audioSource.time);

            onBeatMusic.Invoke();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        CheckPlaying();
    }

    private void CheckPlaying()
    {
        if (audioSource.isPlaying)
        {
            whenPlayingMusic.Invoke(audioSource.time);

            prevPlaying = true;
        }
        else
        {
            if (prevPlaying)
            {
                onEndMusic.Invoke();
            }

            prevPlaying = false;
        }
    }

    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioLength = clip.length;

        onSetClip.Invoke(clip);
    }

    public void PlayAudio()
    {
        audioSource.Play();

        onPlayMusic.Invoke();
    }

    public void SaveToFile()
    {
        string clipName = audioSource.clip.name;

        var data = new BeatDataSaveType();
        data.beatDataSaveTypeList = beatDataList;
        var jsonData = JsonUtility.ToJson(data);

        string filePath = Application.streamingAssetsPath;
        string[] splitedPath = filePath.Split('/');

        filePath = "";
        for (int i = 0; i < splitedPath.Length - 1; ++i)
        {
            filePath += splitedPath[i] + "/";
        }

        filePath += "Resources/BeatDatas";

        //Debug.Log();
        Debug.Log(clipName);

        string str = File.ReadAllText(filePath + "/" + clipName + " BeatData.txt");
        File.WriteAllText(filePath + "/" + clipName + " BeatData.txt", jsonData);

        if (str != "")
        {
            File.WriteAllText(filePath + "/BackUp/" + clipName + " BeatData.txt", str);
        }

        beatDataList = new();
    }
}
