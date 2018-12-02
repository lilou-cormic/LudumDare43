using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : Token
{
    public Text PlayerCounterText;

    public int Count { get; set; } = 1;

    private bool MustRemove = false;

    public void HighlightTiles()
    {
        foreach (var tile in GetAvailableTiles())
        {
            tile.HighlightTile(GameManager.Instance.IsSelectingTile);
        }
    }

    public override void MoveToTile(Tile tile)
    {
        GameManager.Instance.IsPlayersTurn = false;
        GameManager.Instance.IsSelectingTile = false;

        HighlightTiles();
        CurrentTile.Player = null;

        base.MoveToTile(tile);

        if (CurrentTile.Player == null)
        {
            CurrentTile.Player = this;
        }
        else
        {
            CurrentTile.Player.Count += Count;

            if (CurrentTile.IsGoal)
                GameManager.Instance.PlayerSafe += Count;

            MustRemove = true;
        }
    }
    protected override void OnDoneMoving()
    {
        if (MustRemove)
            RemovePlayer();

        GameManager.Instance.EndPlayerTurn();
    }

    private void RemovePlayer()
    {
        GameManager.Instance.Players.Remove(this);
        Destroy(gameObject);
    }

    public void Die()
    {
        Debug.Log($"{name} Died [{CurrentTile.name}]");

        GameManager.Instance.PlayerDead += Count;

        CurrentTile.Player = null;
        RemovePlayer();
    }

    private void LateUpdate()
    {
        PlayerCounterText.text = Count.ToString() ?? "";
    }
}
