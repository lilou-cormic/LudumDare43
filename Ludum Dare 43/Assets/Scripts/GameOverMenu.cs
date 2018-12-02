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
        GameOverPanel.SetActive(true);
    }
}
