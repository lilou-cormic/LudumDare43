using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle MusicAToggle;
    public Toggle MusicBToggle;
    public Toggle MusicNoneToggle;
    public Slider MusicVolumeSlider;

    public Toggle SoundEffecsOnToggle;
    public Toggle SoundEffecsOffToggle;
    public Slider SoundEffectsVolumeSlider;

    private void OnEnable()
    {
        MusicManager.CanPlaySamples = false;

        MusicAToggle.isOn = MusicManager.PlayMusicA;
        MusicBToggle.isOn = MusicManager.PlayMusicB;
        MusicNoneToggle.isOn = MusicManager.PlayNoMusic;
        MusicVolumeSlider.value = MusicManager.Volume;

        SoundEffecsOnToggle.isOn = SoundEffectManager.IsOn;
        SoundEffecsOffToggle.isOn = !SoundEffectManager.IsOn;
        SoundEffectsVolumeSlider.value = SoundEffectManager.Volume;

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
        MusicManager.Volume = MusicVolumeSlider.value;

        SoundEffectManager.IsOn = SoundEffecsOnToggle.isOn;
        SoundEffectManager.Volume = SoundEffectsVolumeSlider.value;


        MusicManager.OnMusicChanged();
    }
}
