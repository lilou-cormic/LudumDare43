using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    public static bool IsGamePaused { get; private set; } = false;

    private void OnEnable()
    {
        IsGamePaused = false;
        PausePanel.SetActive(false);
    }

    public void ResumeGame()
    {
        if (GameOverMenu.IsGameOver)
            return;

        PausePanel.SetActive(false);

        IsGamePaused = PausePanel.activeSelf;

        Time.timeScale = 1;
        MusicManager.MainInstance.AudioSource.volume = MusicManager.Volume;
    }

    private void OnDisable()
    {
        if (MusicManager.MainInstance?.AudioSource != null)
            MusicManager.MainInstance.AudioSource.volume = MusicManager.Volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOverMenu.IsGameOver)
        {
            PausePanel.SetActive(false);
            IsGamePaused = false;
            Time.timeScale = 1;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            PausePanel.SetActive(!PausePanel.activeSelf);

            IsGamePaused = PausePanel.activeSelf;

            if (IsGamePaused)
            {
                Time.timeScale = 0;
                MusicManager.MainInstance.AudioSource.volume *= 0.5f;
            }
            else
            {
                Time.timeScale = 1;
                MusicManager.MainInstance.AudioSource.volume = MusicManager.Volume;
            }
        }
    }

}
