using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private const int MAX_MUSIC_VOLUMN = 10;
    private static float musicTime;
    private static int musicVolumn = 4;

    private AudioSource backgroundMusicAudioSource;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        backgroundMusicAudioSource = GetComponent<AudioSource>();
        backgroundMusicAudioSource.time = musicTime;
    }

    void Start()
    {
        backgroundMusicAudioSource.volume = GetNomalizedMusicVolumn();
    }

    // Update is called once per frame
    void Update()
    {
        musicTime = backgroundMusicAudioSource.time;
    }

    private float GetNomalizedMusicVolumn()
    {
        return ((float)musicVolumn) / MAX_MUSIC_VOLUMN;
    }

    #region PUBLIC_METHODS

    public int GetMusicVolumn() => musicVolumn;

    public void ChangedMusicVolumn()
    {
        musicVolumn = (musicVolumn + 1) % MAX_MUSIC_VOLUMN;
        backgroundMusicAudioSource.volume = GetNomalizedMusicVolumn();
    }

    #endregion // PUBLIC_METHODS
}
