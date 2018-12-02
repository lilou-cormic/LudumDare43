using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOverPanel;

    public static bool IsGameOver { get; set; }

    private void OnEnable()
    {
        IsGameOver = false;
        GameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (GameManager.Instance.PlayerSafe >= GameManager.Instance.MaxPlayer / 2)
            SoundEffectManager.PlayGoodGameOverClip();
        else
            SoundEffectManager.PlayBadGameOverClip();

        GameOverPanel.SetActive(true);
    }
}
