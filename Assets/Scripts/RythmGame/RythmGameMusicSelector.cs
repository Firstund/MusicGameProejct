using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmGameMusicSelector : MonoBehaviour
{
    // RythmGame을 시작하기 전에 Music과 해당 Music의 BeatData를 Select하는 역할을 함.
    void Start()
    {
        SelectMusic("13. 새벽의 다짐 (Resolution of Dawn) - TTRM"); // For Test
    }

    void Update()
    {
        
    }

    private AudioClip LoadMusicClip(string musicFileName){
        AudioClip result = null;

        result = Resources.Load<AudioClip>("MusicFiles/" + musicFileName);

        return result;
    }

    public void SelectMusic(string musicFileName)
    {
        BeatDataManager.Instance.LoadBeatData(musicFileName);       // 해당 MusicFile의 BeatData를 Load.
        AudioClip musicAudioClip = LoadMusicClip(musicFileName);    // 해당 MusicFile의 AudioClipt을 Load.

        AudioManager.Instance.SetMainBGMClip(musicAudioClip);
    }
}
