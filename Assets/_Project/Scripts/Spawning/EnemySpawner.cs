using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// This script controls enemy waves.
// It starts one wave, waits until all enemies die,
// then starts the next wave with increased difficulty.
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]

    // Enemy prefab that will be spawned during waves.
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]

    // Possible positions where enemies can appear.
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Settings")]

    // Current wave number.
    [SerializeField] private int currentWave = 0;

    // Number of enemies in the first wave.
    [SerializeField] private int baseEnemiesPerWave = 3;

    // How many extra enemies are added each new wave.
    [SerializeField] private int enemiesAddedPerWave = 2;

    // Delay between each enemy spawn inside the same wave.
    [SerializeField] private float spawnDelay = 0.6f;

    // Delay before starting the next wave after all enemies are defeated.
    [SerializeField] private float timeBetweenWaves = 2f;

    [Header("Events")]

    [SerializeField] private UnityEvent<int, int> onWaveInfoChanged;

    // Invoked before a wave starts spawning enemies.
    // Sends the wave number and total enemies that will spawn in that wave.
    [SerializeField] private UnityEvent<int, int> onWaveAnnouncement;

    // Tracks how many enemies are alive right now.
    private int enemiesAlive = 0;

    // Tracks whether a wave is currently spawning enemies.
    private bool isSpawningWave = false;

    private void Start()
    {
        // Update UI with initial values before the first wave starts.
        NotifyWaveInfoChanged();

        // Start the first wave when the scene begins.
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        // Prevent multiple waves from starting at the same time.
        if (isSpawningWave)
        {
            yield break;
        }

        // Move to the next wave first so we can announce it during the waiting time.
        currentWave++;

        // Calculate how many enemies this wave should spawn.
        int enemiesToSpawn = baseEnemiesPerWave + (currentWave - 1) * enemiesAddedPerWave;

        // Announce the next wave before enemies start spawning.
        onWaveAnnouncement?.Invoke(currentWave, enemiesToSpawn);

        // Update UI with the new wave number.
        NotifyWaveInfoChanged();

        // Wait before spawning enemies.
        // This gives the player time to prepare and read the announcement.
        yield return new WaitForSeconds(timeBetweenWaves);

        // Mark that we are currently spawning this wave.
        isSpawningWave = true;

        // Spawn enemies one by one with a small delay.
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();

            // Update UI after each enemy is spawned.
            NotifyWaveInfoChanged();

            // Wait before spawning the next enemy.
            yield return new WaitForSeconds(spawnDelay);
        }

        // Finished spawning this wave.
        isSpawningWave = false;

        // If all enemies somehow died while the wave was still spawning,
        // start the next wave after the normal delay.
        if (enemiesAlive == 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    private void SpawnEnemy()
    {
        // Do not spawn if the prefab is missing.
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing enemyPrefab reference.");
            return;
        }

        // Do not spawn if there are no spawn points assigned.
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner has no spawn points assigned.");
            return;
        }

        // Pick a random spawn point.
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        // Store the selected spawn position.
        Vector3 spawnPosition = selectedSpawnPoint.position;

        // Force Z to -1 so enemies appear in front of the arena.
        spawnPosition.z = -1f;

        // Create the enemy in the scene.
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Increase alive count.
        enemiesAlive++;

        // Listen to this enemy's death event.
        Health enemyHealth = enemy.GetComponent<Health>();

        if (enemyHealth != null)
        {
            enemyHealth.OnDeath.AddListener(HandleEnemyDeath);
        }
    }

    private void HandleEnemyDeath()
    {
        // Reduce alive count, but never below zero.
        enemiesAlive = Mathf.Max(enemiesAlive - 1, 0);

        // Update UI because the number of enemies alive changed.
        NotifyWaveInfoChanged();

        // If no enemies are alive and we are not currently spawning,
        // the wave is complete.
        if (enemiesAlive == 0 && !isSpawningWave)
        {
            StartCoroutine(StartNextWave());
        }
    }

    private void NotifyWaveInfoChanged()
    {
        // Notify listeners with current wave and enemies alive.
        onWaveInfoChanged?.Invoke(currentWave, enemiesAlive);
    }
}