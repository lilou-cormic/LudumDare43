using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Token
{
    public SpriteRenderer SpriteRenderer;

    public override IEnumerable<Tile> GetAvailableTiles()
    {
        return CurrentTile.AdjacentTiles.Where(x => !x.IsGoal && x.Enemy == null);
    }

    public override void MoveToTile(Tile tile)
    {
        GameManager.Instance.IsEnemysTurn = false;

        CurrentTile.Enemy = null;

        base.MoveToTile(tile);

        CurrentTile.Enemy = this;
    }

    protected override void OnDoneMoving()
    {
        if (CurrentTile.Player != null)
        {
            CurrentTile.Player.Die();
            Die();
        }

        GameManager.Instance.EndEnemyTurn();
    }

    private Tile _TargetTile;

    public Tile TargetTile
    {
        get { return _TargetTile; }
        set
        {
            _TargetTile = value;
            TargetPlayer = _TargetTile?.Player;
        }
    }

    public Player TargetPlayer { get; private set; } = null;

    public void MoveTowardsPlayer(Player player)
    {
        Vector3 playerPos = player.CurrentTile.transform.position;

        Tile bestTile = null;
        float bestDistance = float.MaxValue;

        foreach (var tile in GetAvailableTiles())
        {
            float dist = Mathf.Abs(tile.transform.position.x - playerPos.x) + Mathf.Abs(tile.transform.position.y - playerPos.y);

            if (dist < bestDistance)
            {
                bestTile = tile;
                bestDistance = dist;
            }
        }

        TargetTile = bestTile;
        GameManager.Instance.EnemyToMove = this;
    }

    public void MoveRandomly()
    {
        Tile[] tiles = GetAvailableTiles().ToArray();

        TargetTile = tiles[UnityEngine.Random.Range(0, tiles.Length)];
        GameManager.Instance.EnemyToMove = this;
    }

    public void Die()
    {
        //Debug.Log($"{name} Died [{CurrentTile.name}]");

        CurrentTile.Enemy = null;
        GameManager.Instance.Enemies.Remove(this);
        Destroy(gameObject);

        GameManager.Instance.EndPlayerTurn();
    }
}
