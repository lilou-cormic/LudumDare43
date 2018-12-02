using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionsPanel = null;
    public GameObject CreditsPanel = null;

    public MusicManager MainMusicPlayer = null;

    private void Start()
    {
        if (OptionsPanel != null)
            OptionsPanel.SetActive(false);

        if (CreditsPanel != null)
            CreditsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowHideOptions(bool show)
    {
        if (MainMusicPlayer != null)
            MainMusicPlayer.gameObject.SetActive(!show);


        OptionsPanel?.SetActive(show);
    }

    public void ShowHideCredits(bool show)
    {
        CreditsPanel?.SetActive(show);
    }
}
