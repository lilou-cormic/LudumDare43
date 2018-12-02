using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager MainInstance { get; set; }

    public static bool IsOn { get; set; } = true;
    private static float _Volume = 1f;
    public static float Volume { get { return _Volume; } set { _Volume = value; if (MainInstance?.AudioSource != null) MainInstance.AudioSource.volume = value; } }

    public AudioSource AudioSource;

    public AudioClip GoalClip;
    public AudioClip DeathClip;
    public AudioClip GoodGameOverClip;
    public AudioClip BadGameOverClip;

    public bool IsMainPlayer = false;

    private void Start()
    {
        if (IsMainPlayer)
        {
            if (MainInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            AudioSource.volume = _Volume;
            MainInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void PlayGoalClip()
    {
        if (IsOn)
            MainInstance.AudioSource.PlayOneShot(MainInstance.GoalClip);
    }

    public static void PlayDeathClip()
    {
        if (IsOn)
            MainInstance.AudioSource.PlayOneShot(MainInstance.DeathClip);
    }

    public static void PlayGoodGameOverClip()
    {
        if (IsOn)
            MainInstance.AudioSource.PlayOneShot(MainInstance.GoodGameOverClip);
    }

    public static void PlayBadGameOverClip()
    {
        if (IsOn)
            MainInstance.AudioSource.PlayOneShot(MainInstance.BadGameOverClip);
    }
}
