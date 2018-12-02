using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Token Prefab;

    public Tile Tile;

    public T Spawn<T>()
        where T : Token
    {
        T token = Instantiate(Prefab, transform.position, Quaternion.identity, transform) as T;
        token.CurrentTile = Tile;
        token.name = token.GetType().Name + Time.time.ToString();

        return token;
    }
}
