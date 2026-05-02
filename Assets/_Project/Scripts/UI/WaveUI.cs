using TMPro;
using UnityEngine;

// This script updates the wave UI.
// It does not control spawning logic.
// It only receives wave data and displays it.
public class WaveUI : MonoBehaviour
{
    [Header("UI References")]

    // Text used to display the current wave number.
    [SerializeField] private TMP_Text waveText;

    // Text used to display how many enemies are still alive.
    [SerializeField] private TMP_Text enemiesLeftText;

    private void Awake()
    {
        // Initialize the UI with default values.
        // This prevents empty text before the first wave starts.
        UpdateWaveText(0);
        UpdateEnemiesLeftText(0);
    }

    // Updates only the wave number text.
    public void UpdateWaveText(int currentWave)
    {
        if (waveText == null)
        {
            return;
        }

        waveText.text = $"Wave: {currentWave}";
    }

    // Updates only the enemies left text.
    public void UpdateEnemiesLeftText(int enemiesLeft)
    {
        if (enemiesLeftText == null)
        {
            return;
        }

        enemiesLeftText.text = $"Enemies Alive: {enemiesLeft}";
    }

    // Updates both values at the same time.
    // This is useful when the spawner wants to refresh the full UI.
    public void UpdateWaveInfo(int currentWave, int enemiesLeft)
    {
        UpdateWaveText(currentWave);
        UpdateEnemiesLeftText(enemiesLeft);
    }
}