using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public GameObject Highlight;

    public Tile[] AdjacentTiles;

    public Player Player { get; set; }

    public Enemy Enemy { get; set; }

    public bool IsGoal;

    private void Awake()
    {
        Highlight.SetActive(false);
    }

    public void HighlightTile(bool highlight)
    {
        Highlight.SetActive(highlight);
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.IsPlayersTurn)
            return;

        if (GameManager.Instance.IsSelectingTile)
        {
            if (Highlight.activeSelf)
            {
                GameManager.Instance.SelectedPlayer.MoveToTile(this);
                return;
            }
            else if (GameManager.Instance.SelectedPlayer == Player)
            {
                GameManager.Instance.IsSelectingTile = false;
                GameManager.Instance.SelectedPlayer.HighlightTiles();

                GameManager.Instance.SelectedPlayer = null;
                return;
            }
        }

        GameManager.Instance.IsSelectingTile = false;
        GameManager.Instance.SelectedPlayer?.HighlightTiles();

        if (!IsGoal)
        {
            if (Player != null)
            {
                GameManager.Instance.SelectedPlayer = Player;
                GameManager.Instance.IsSelectingTile = Player.CanMove();
                Player.HighlightTiles();
            }
            //else if (Enemy != null)
            //{
            //    Enemy.Die();
            //}
        }
    }
}
