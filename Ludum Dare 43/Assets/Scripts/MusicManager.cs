using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager MainInstance { get; private set; }

    public static bool PlayMusicA { get; set; } = true;
    public static bool PlayMusicB { get; set; }
    public static bool PlayNoMusic { get; set; }

    public static float Volume { get; set; } = 0.75f;

    public AudioSource AudioSource;

    public AudioClip MainMusicClip;
    public AudioClip AlternateMusicClip;

    private bool IsPlayingSample = false;

    private float SamplePlayTime = 5f;
    private float SamplePlayTimeLeft = 0f;

    public static bool CanPlaySamples = false;

    public bool IsMainPlayer = false;

    public static event Action MusicChanged;

    public static void OnMusicChanged()
    {
        MusicChanged?.Invoke();
    }

    private void Awake()
    {
        MusicChanged += MusicManager_MusicChanged;
    }

    private void Start()
    {
        if (IsMainPlayer)
        {
            if (MainInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            MainInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        MusicChanged -= MusicManager_MusicChanged;
    }

    private void MusicManager_MusicChanged()
    {
        if (IsMainPlayer)
        {
            if (PlayMusicA)
                AudioSource.clip = MainMusicClip;
            else if (PlayMusicB)
                AudioSource.clip = AlternateMusicClip;
            else
                AudioSource.clip = null;

            if (AudioSource.clip)
                AudioSource.Play();
            else
                AudioSource.Stop();
        }
    }

    private void OnEnable()
    {
        IsPlayingSample = false;
        CanPlaySamples = false;

        AudioSource.volume = Volume;
        if (PauseMenu.IsGamePaused)
            AudioSource.volume *= 0.5f;

        MusicManager_MusicChanged();
    }

    public void PlayMainMusicSample()
    {
        StopPlayingMusicSample();

        if (!CanPlaySamples)
            return;

        AudioSource.PlayOneShot(MainMusicClip);
        SamplePlayTimeLeft = SamplePlayTime;
        IsPlayingSample = true;
    }

    public void PlayAlternateMusicSample()
    {
        StopPlayingMusicSample();

        if (!CanPlaySamples)
            return;

        AudioSource.PlayOneShot(AlternateMusicClip);
        SamplePlayTimeLeft = SamplePlayTime;
        IsPlayingSample = true;
    }

    public void StopPlayingMusicSample()
    {
        AudioSource.Stop();
        IsPlayingSample = false;
    }

    private void Update()
    {
        if (IsPlayingSample)
        {
            SamplePlayTimeLeft -= Time.deltaTime;

            if (SamplePlayTimeLeft <= 0)
                StopPlayingMusicSample();
        }
    }
}
