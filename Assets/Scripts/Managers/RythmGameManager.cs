using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmGameManager : MonoSingleton<RythmGameManager>
{
    [SerializeField]
    private bool isPlayingRythmGame = false;
    public bool IsPlayingRythmGame
    {
        get { return isPlayingRythmGame;}
    }

    void Update()
    {
        if(isPlayingRythmGame)
        {
            BeatNoteManager.Instance.CheckSpawnBeatNote(AudioManager.Instance.GetMainBGMAudioSource().time);
        }
    }

    public void StartRythmGame()
    {
        // BeatDataManager의 CurBeatData는 별도의 SelectorScript를 통해 Set 된다.

        AudioManager.Instance.PlayMainBGM();
        BeatNoteManager.Instance.OnStartRythmGame(BeatDataManager.Instance.GetCurBeatData());

        isPlayingRythmGame = true;
    }
}
