using UnityEngine;

// This script spawns pickups at random spawn points.
// It supports multiple pickup prefabs, but only one active pickup at a time.
// This keeps the game balanced and prevents too many power-ups from appearing.
public class PickupSpawner : MonoBehaviour
{
    [Header("Pickup Settings")]

    // List of possible pickup prefabs.
    // Example: HealthPickup and FireRatePickup.
    [SerializeField] private GameObject[] pickupPrefabs;

    [Header("Spawn Settings")]

    // Possible positions where pickups can appear.
    [SerializeField] private Transform[] spawnPoints;

    // Time in seconds between spawn attempts.
    [SerializeField] private float spawnInterval = 8f;

    // If true, the first pickup appears after a delay.
    // If false, one pickup appears immediately at the start.
    [SerializeField] private bool waitBeforeFirstSpawn = true;

    // Internal timer used to control when the next pickup can spawn.
    private float spawnTimer;

    // Stores the pickup currently active in the scene.
    // We use this to make sure only one pickup exists at a time.
    private GameObject activePickup;

    // Stores the last pickup index used.
    // This helps prevent the same pickup from appearing repeatedly.
    private int lastPickupIndex = -1;

    private void Start()
    {
        // If we do not want to wait, spawn the first pickup immediately.
        if (!waitBeforeFirstSpawn)
        {
            SpawnPickup();
        }
    }

    private void Update()
    {
        // If there is already an active pickup, do not spawn another one.
        if (activePickup != null)
        {
            return;
        }

        // Increase the timer while no pickup is active.
        spawnTimer += Time.deltaTime;

        // Spawn a new pickup when the timer reaches the interval.
        if (spawnTimer >= spawnInterval)
        {
            SpawnPickup();

            // Reset the timer after spawning.
            spawnTimer = 0f;
        }
    }

    private void SpawnPickup()
    {
        // Do not spawn if no pickup prefabs are assigned.
        if (pickupPrefabs == null || pickupPrefabs.Length == 0)
        {
            Debug.LogWarning("PickupSpawner has no pickup prefabs assigned.");
            return;
        }

        // Do not spawn if there are no spawn points assigned.
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("PickupSpawner has no spawn points assigned.");
            return;
        }

        // Choose a random pickup type.
        // If there is more than one pickup type, avoid choosing the same one twice in a row.
        int randomPickupIndex = GetRandomPickupIndex();

        GameObject selectedPickupPrefab = pickupPrefabs[randomPickupIndex];

        // Store this index as the last used pickup.
        lastPickupIndex = randomPickupIndex;

        // Safety check in case one slot in the array is empty.
        if (selectedPickupPrefab == null)
        {
            Debug.LogWarning("PickupSpawner has an empty pickup prefab slot.");
            return;
        }

        // Choose a random spawn point.
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomSpawnIndex];

        // Store the spawn position.
        Vector3 spawnPosition = selectedSpawnPoint.position;

        // Force Z to -1 so the pickup appears in front of the arena.
        spawnPosition.z = -1f;

        // Create the pickup and keep a reference to it.
        activePickup = Instantiate(selectedPickupPrefab, spawnPosition, Quaternion.identity);
    }
    private int GetRandomPickupIndex()
    {
        // If there is only one pickup type, return it directly.
        if (pickupPrefabs.Length == 1)
        {
            return 0;
        }

        int randomIndex;

        // Keep choosing until we get a different pickup from the last one.
        // With only two pickups, this effectively alternates them.
        do
        {
            randomIndex = Random.Range(0, pickupPrefabs.Length);
        }
        while (randomIndex == lastPickupIndex);

        return randomIndex;
    }
}