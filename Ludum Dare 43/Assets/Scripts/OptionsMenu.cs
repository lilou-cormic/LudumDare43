using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle MusicAToggle;
    public Toggle MusicBToggle;
    public Toggle MusicNoneToggle;

    private void OnEnable()
    {
        MusicManager.CanPlaySamples = false;

        MusicAToggle.isOn = MusicManager.PlayMusicA;
        MusicBToggle.isOn = MusicManager.PlayMusicB;
        MusicNoneToggle.isOn = MusicManager.PlayNoMusic;

        MusicManager.CanPlaySamples = true;
    }

    private void OnDisable()
    {
        MusicManager.CanPlaySamples = false;
    }

    public void AcceptOptions()
    {
        MusicManager.PlayMusicA = MusicAToggle.isOn;
        MusicManager.PlayMusicB = MusicBToggle.isOn;
        MusicManager.PlayNoMusic = MusicNoneToggle.isOn;

        MusicManager.OnMusicChanged();
    }
}
