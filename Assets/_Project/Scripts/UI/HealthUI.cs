using TMPro;
using UnityEngine;

// This script updates the health UI text.
// It does not calculate health and does not apply damage.
// It only receives health values and displays them.
public class HealthUI : MonoBehaviour
{
    [Header("UI References")]

    // TextMeshPro component used to display the player's health.
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        // If the reference was not assigned manually,
        // try to get TMP_Text from the same GameObject.
        if (healthText == null)
        {
            healthText = GetComponent<TMP_Text>();
        }
    }

    // This method will be called by the Health component event.
    // It receives current health and max health.
    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        // Safety check to avoid null reference errors.
        if (healthText == null)
        {
            return;
        }

        // Update the visible UI text.
        healthText.text = $"HP: {currentHealth} / {maxHealth}";
    }
}