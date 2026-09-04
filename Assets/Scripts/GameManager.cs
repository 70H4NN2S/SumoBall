using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] pickUps;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameCanvasHandler gameCanvasHandler;

    [Header("Spawning")]
    [SerializeField] private float spawnRange = 9f;

    [Header("Starting Round")]
    [SerializeField] private int startingEnemyCount = 2;
    [SerializeField] private int startingPowerUpCount = 0;

    private int enemiesCount;
    private int powerUpCount;
    private int round;

    private GameState gameState;

    private enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    private void Awake()
    {
        // Time.timeScale survives scene changes, so explicitly restore it.
        Time.timeScale = 1f;

        enemiesCount = startingEnemyCount;
        powerUpCount = startingPowerUpCount;

        gameState = GameState.Playing;
    }

    private void Start()
    {
        StartNextRound();
    }

    private void Update()
    {
        if (gameState != GameState.Playing)
            return;

        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        RemovePowerUps();

        SpawnEnemies();
        SpawnPowerUps();

        round++;
        gameCanvasHandler.DrawRound(round);

        enemiesCount++;
        powerUpCount++;
    }

    private void RemovePowerUps()
    {
        GameObject[] activePickUps = GameObject.FindGameObjectsWithTag("PickUp");

        foreach (GameObject pickUp in activePickUps)
        {
            Destroy(pickUp);
        }
    }

    private void SpawnEnemies()
    {
        int spawnedEnemies = 0;

        while (spawnedEnemies < enemiesCount)
        {
            int remaining = enemiesCount - spawnedEnemies;

            int maxIndexExclusive = Mathf.Min(
                enemies.Length,
                remaining
            );

            int index = Random.Range(0, maxIndexExclusive);

            spawnedEnemies += index + 1;

            Instantiate(
                enemies[index],
                GetSpawnPosition(5f),
                Quaternion.identity
            );
        }
    }

    private void SpawnPowerUps()
    {
        int spawnedPowerUps = 0;

        while (spawnedPowerUps < powerUpCount)
        {
            int remaining = powerUpCount - spawnedPowerUps;

            int maxIndexExclusive = Mathf.Min(
                pickUps.Length,
                remaining
            );

            int index = Random.Range(0, maxIndexExclusive);

            spawnedPowerUps += index + 1;

            Instantiate(
                pickUps[index],
                GetSpawnPosition(),
                Quaternion.identity
            );
        }
    }

    private Vector3 GetSpawnPosition(float y = 0f)
    {
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        return new Vector3(x, y, z);
    }

    public void PauseGame()
    {
        gameState = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        gameState = GameState.Playing;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        Time.timeScale = 0f;

        gameCanvasHandler.GameOver();
    }
}