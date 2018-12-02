using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Token : MonoBehaviour
{
    public Tile CurrentTile { get; set; }

    public virtual void MoveToTile(Tile tile)
    {
        Debug.Log($"{name}: {CurrentTile.name} - {tile.name}");

        CurrentTile = tile;

        transform.position = CurrentTile.transform.position;
    }

    public virtual IEnumerable<Tile> GetAvailableTiles()
    {
        return CurrentTile.AdjacentTiles.Where(x => x.Enemy == null);
    }

    public bool CanMove()
    {
        return GetAvailableTiles().Any();
    }
}
