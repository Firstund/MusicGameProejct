using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeatNoteType
{
    Up,
    Down,
    Dual,
    LongUp,
    LongDown,
    LongDual,
}
public class BeatNoteSpawnData
{
    public float spawnTime;
    public float reachTime;

    public float spectrumValue;
}
public class BeatNoteManager : MonoSingleton<BeatNoteManager>
{
    [SerializeField]
    private BeatNote beatNote = null; // Instantiate에 쓰일 원본 // 후에 여러 BeatNoteType에 관한 처리를 꼭 넣어둘것

    private List<BeatNote> beatNoteList = new();
    private List<BeatNoteSpawnData> beatNoteSpawnDataList = new();

    [SerializeField]
    private Transform beatNoteRoot = null;

    [SerializeField]
    private Transform beatNoteStartPosition = null;
    [SerializeField]
    private Transform beatNoteEndPosition = null;

    // private Vector3 beatNoteMoveDir => (beatNoteEndPosition.position - beatNoteStartPosition.position).normalized;

    [SerializeField]
    private int nextSpawnIndex = 0;

    [SerializeField]
    private float beatNoteMoveSpeed = 0f;
    public float BeatNoteMoveSpeed
    {
        get
        {
            return beatNoteMoveSpeed;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (RythmGameManager.Instance.IsPlayingRythmGame)
        {
            SetBeatNotePositions();
        }
    }

    private void SetBeatNotePositions()
    {
        for (int i = 0; i < beatNoteList.Count; ++i)
        {
            // Vector3 curBeatNotePos = beatNoteList[i].transform.position;
            // Vector3 moveDelta  = Vector3.zero;

            // moveDelta = beatNoteMoveDir * (beatNoteMoveSpeed * BeatDataManager.Instance.XLengthPerSec * Time.deltaTime);
            // moveDelta.y = 0f;
            // moveDelta.z = 0f;

            // curBeatNotePos += moveDelta;

            // beatNoteList[i].transform.position = curBeatNotePos;

            var curBeatNote = beatNoteList[i];

            curBeatNote.transform.position = Vector3.Lerp(beatNoteStartPosition.position, beatNoteEndPosition.position, curBeatNote.ReachToTargetTimer / curBeatNote.ReachToTargetTime);
        }
    }

    public void OnStartRythmGame(BeatDataSaveType beatDataSaveType)
    {
        nextSpawnIndex = 0;
        beatNoteSpawnDataList = new();
        List<BeatDataType> beatDataList = beatDataSaveType.beatDataSaveTypeList;

        float timeOffset = 0f;
        for (int i = 0; i < beatDataList.Count; ++i)
        {
            var beatData = beatDataList[i];

            if(i == 0)
            {
                timeOffset = SetBeatNoteSpawnData(beatData.beatTime, beatData.spectrumValue, timeOffset, true);
            }
            else
            {
                SetBeatNoteSpawnData(beatData.beatTime, beatData.spectrumValue, timeOffset);
            }
        }
    }

    // Call On StartGame
    private float SetBeatNoteSpawnData(float reachTime, float spectrumValue, float timeOffset, bool isFirstBeatNote = false)
    {
        BeatNoteSpawnData noteSpawnData = new BeatNoteSpawnData();
        float resultTimeOffset = 0f;
        float distance = Vector3.Distance(beatNoteStartPosition.position, beatNoteEndPosition.position);
        reachTime += timeOffset;
        float spawnTime = reachTime - (distance / (BeatDataManager.Instance.XLengthPerSec * beatNoteMoveSpeed));

        if(isFirstBeatNote && spawnTime < 0f)
        {
            resultTimeOffset = -spawnTime;
            reachTime += resultTimeOffset;
            spawnTime = 0f;
        }

        noteSpawnData.spawnTime = spawnTime;
        noteSpawnData.reachTime = reachTime;

        noteSpawnData.spectrumValue = spectrumValue;

        beatNoteSpawnDataList.Add(noteSpawnData);

        return resultTimeOffset;
    }

    private void SpawnBeatNoteObject(BeatNoteSpawnData beatNoteSpawnData)
    {
        Debug.Log(AudioManager.Instance.GetMainBGMAudioSource().time);

        BeatNote bn = Instantiate(beatNote);

        bn.transform.SetParent(beatNoteRoot);
        bn.transform.localScale = Vector3.one;
        bn.transform.localRotation = Quaternion.identity;
        bn.transform.position = beatNoteStartPosition.position;

        bn.Init(beatNoteSpawnData);
        beatNoteList.Add(bn);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void CheckSpawnBeatNote(float time)
    {
        var beatNoteSpawnData = beatNoteSpawnDataList[nextSpawnIndex];

        if (beatNoteSpawnData.spawnTime <= time)
        {
            // Debug.Log(beatNoteSpawnData.spawnTime);
            // spawnBeatNote
            SpawnBeatNoteObject(beatNoteSpawnData);

            nextSpawnIndex++;
        }
    }
}
