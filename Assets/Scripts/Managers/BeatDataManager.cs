using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDataManager : MonoSingleton<BeatDataManager>
{
    private BeatDataSaveType curBeatData;
    
    [SerializeField]
    private float xLengthPerSec = 1f;
    public float XLengthPerSec
    {
        get{ return xLengthPerSec; }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadBeatData(string musicFileName)
    {
        var textAsset = Resources.Load<TextAsset>("BeatDatas/" + musicFileName + " BeatData");
        curBeatData = JsonUtility.FromJson<BeatDataSaveType>(textAsset.text);
        // Resources.Load
    }

    public BeatDataSaveType GetCurBeatData()
    {
        return curBeatData;
    }
}
