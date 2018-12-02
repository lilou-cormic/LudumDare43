using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameOverMenu GameOverMenu;

    public Spawner PlayerSpawner;
    public Spawner[] EnemySpawners;

    public Tile GoalTile;
    public Player GoalPlayer;

    public bool IsPlayersTurn { get; set; }
    public bool IsSelectingTile { get; set; }
    public bool IsEnemysTurn { get; set; }

    public Player SelectedPlayer { get; set; }

    public List<Player> Players = new List<Player>();

    public List<Enemy> Enemies = new List<Enemy>();

    public int PlayerCounter { get; set; } = 0;

    public int MaxPlayer { get; private set; } = 100;

    public int PlayerLeft { get; set; } = 100;
    public int PlayerDead { get; set; } = 0;
    public int PlayerSafe { get; set; } = 0;

    public Text PlayerLeftText;
    public Text PlayerDangerText;
    public Text PlayerDeadText;
    public Text PlayerSafeText;

    private bool MustWaitToStartPlayerTurn = false;
    private bool MustWaitToMoveEnemy = false;

    public static int HighScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        PlayerLeft = MaxPlayer;
        PlayerDead = 0;
        PlayerSafe = 0;

        PlayerCounter = 0;
        GoalTile.Player = GoalPlayer;
        GoalPlayer.CurrentTile = GoalTile;
        GoalPlayer.Count = 0;

        IsPlayersTurn = false;
        IsSelectingTile = false;
        IsEnemysTurn = false;

        MustWaitToStartPlayerTurn = false;
        MustWaitToMoveEnemy = false;

        SpawnPlayer();
    }

    public void EndPlayerTurn()
    {
        IsPlayersTurn = false;
        IsSelectingTile = false;

        if (PlayerLeft == 0 && Players.Count == 0)
        {
            if (PlayerSafe > HighScore)
                HighScore = PlayerSafe;

            GameOverMenu.ShowGameOver();
        }

        SpawnEnemy();
    }

    public void EndEnemyTurn()
    {
        IsEnemysTurn = false;

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (PlayerLeft == 0 && Players.Count == 0)
            return;

        IsSelectingTile = false;
        IsEnemysTurn = false;

        if (PlayerLeft > 0)
        {
            if (PlayerSpawner.Tile.Enemy == null)
            {
                if (PlayerSpawner.Tile.Player == null)
                {
                    Player player = PlayerSpawner.Spawn<Player>();
                    player.CurrentTile.Player = player;

                    Players.Add(player);
                }
                else
                {
                    PlayerSpawner.Tile.Player.Count++;
                }
            }

            PlayerCounter++;
            PlayerLeft--;
        }

        DetermineEnemyMovement();

        MustWaitToStartPlayerTurn = true;
    }

    private void StartPlayerTurn()
    {
        if (!MustWaitToStartPlayerTurn)
            return;

        MustWaitToStartPlayerTurn = false;

        IsPlayersTurn = true;
    }

    private void SpawnEnemy()
    {
        IsPlayersTurn = false;
        IsSelectingTile = false;

        IsEnemysTurn = true;

        if (Enemies.Count < 2 && PlayerCounter % 2 == 1)
        {
            var spawners = EnemySpawners.Where(x => x.Tile.Enemy == null).ToArray();

            if (spawners.Length > 0)
            {
                Enemy enemy = spawners[Random.Range(0, spawners.Length)].Spawn<Enemy>();
                enemy.CurrentTile.Enemy = enemy;

                Enemies.Add(enemy);

                if (EnemyToMove?.TargetTile != null)
                    EnemyToMove.TargetTile = null;
                EnemyToMove = null;
            }
        }

        MustWaitToMoveEnemy = true;
    }

    public Enemy EnemyToMove { get; set; }

    private void DetermineEnemyMovement()
    {
        var enemies = Enemies.Where(x => x.CanMove()).ToArray();

        if (enemies.Length == 0)
        {
            EnemyToMove = null;
            return;
        }

        Enemy bestEnemy = null;
        Player bestPlayer = null;
        float bestDistance = float.MaxValue;

        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];

            foreach (var player in Players)
            {
                float dist = Mathf.Abs(enemy.CurrentTile.transform.position.x - player.transform.position.x) + Mathf.Abs(enemy.transform.position.y - player.transform.position.y);

                if (dist < bestDistance || (dist == bestDistance && player.Count > bestPlayer.Count))
                {
                    bestEnemy = enemy;
                    bestPlayer = player;
                    bestDistance = dist;
                }
            }
        }

        if (bestEnemy != null && bestPlayer != null)
            bestEnemy.MoveTowardsPlayer(bestPlayer);
        else
            enemies[Random.Range(0, enemies.Length)].MoveRandomly();
    }

    public void MoveEnemy()
    {
        if (!MustWaitToMoveEnemy)
            return;

        MustWaitToMoveEnemy = false;

        if (EnemyToMove != null && EnemyToMove.TargetTile != null)
        {
            if (EnemyToMove.TargetPlayer != null && EnemyToMove.TargetTile.Player == null && EnemyToMove.GetAvailableTiles().Any(x => x.Player != null && x.Player != EnemyToMove.TargetPlayer))
                EnemyToMove.MoveToTile(EnemyToMove.GetAvailableTiles().First(x => x.Player != EnemyToMove.TargetPlayer && x.Player?.Count == EnemyToMove.GetAvailableTiles().Where(y => y.Player != null && x.Player != EnemyToMove.TargetPlayer).Max(y => y.Player.Count)));
            else
                EnemyToMove.MoveToTile(EnemyToMove.TargetTile);

            EnemyToMove.TargetTile = null;
        }
        else
        {
            EnemyToMove = null;
            EndEnemyTurn();
        }
    }

    private void Update()
    {
        if (MustWaitToStartPlayerTurn)
            StartCoroutine(WaitToStartPlayerTurn());

        if (MustWaitToMoveEnemy)
            StartCoroutine(WaitToMoveEnemy());
    }

    private IEnumerator WaitToStartPlayerTurn()
    {
        yield return new WaitForSeconds(0.1f);

        StartPlayerTurn();
    }

    private IEnumerator WaitToMoveEnemy()
    {
        yield return new WaitForSeconds(0.2f);

        MoveEnemy();
    }

    private void LateUpdate()
    {
        PlayerLeftText.text = "Left: " + PlayerLeft.ToString();
        PlayerDangerText.text = "Danger: " + Players.Where(x => !x.MustRemove).Sum(x => x.Count).ToString();
        PlayerDeadText.text = "Dead: " + PlayerDead.ToString();
        PlayerSafeText.text = "Safe: " + PlayerSafe.ToString();
    }
}
