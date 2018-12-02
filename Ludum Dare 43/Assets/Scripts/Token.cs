using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Token : MonoBehaviour
{
    public Tile CurrentTile { get; set; }

    private bool IsInitialized { get; set; } = false;
    private bool IsMoving { get; set; } = false;

    protected abstract void OnDoneMoving();

    public virtual void MoveToTile(Tile tile)
    {
        Debug.Log($"{name}: {CurrentTile.name} - {tile.name}");

        CurrentTile = tile;

        IsMoving = true;
        //transform.position = CurrentTile.transform.position;
    }

    public virtual IEnumerable<Tile> GetAvailableTiles()
    {
        return CurrentTile.AdjacentTiles.Where(x => x.Enemy == null);
    }

    public bool CanMove()
    {
        return GetAvailableTiles().Any();
    }

    private Vector3 velocity = Vector3.zero;

    public void Update()
    {
        if (!IsInitialized)
        {
            StartCoroutine(Initialize());
            return;
        }

        if (IsMoving)
        {
            if (Vector3.Distance(transform.position, CurrentTile.transform.position) < 0.01)
            {
                transform.position = CurrentTile.transform.position;
                IsMoving = false;
                OnDoneMoving();
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, CurrentTile.transform.position, ref velocity, Time.deltaTime * 2);
        }
    }

    private IEnumerator Initialize()
    {
        yield return IntilializeInternal();

        IsInitialized = true;
        yield return null;
    }

    protected virtual IEnumerator IntilializeInternal()
    {
        yield return null;
    }
}
