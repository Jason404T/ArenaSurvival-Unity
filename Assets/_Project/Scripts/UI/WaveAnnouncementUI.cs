using System.Collections;
using TMPro;
using UnityEngine;

// This script displays a temporary wave announcement on screen.
// It only handles UI presentation.
// The EnemySpawner decides when a new wave starts.
public class WaveAnnouncementUI : MonoBehaviour
{
    [Header("UI References")]

    // Text used to display the wave announcement.
    [SerializeField] private TMP_Text announcementText;

    // CanvasGroup lets us fade the text in and out.
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timing Settings")]

    // How long the message stays fully visible.
    [SerializeField] private float visibleDuration = 1.5f;

    // How long fade in/out takes.
    [SerializeField] private float fadeDuration = 0.25f;

    // Stores the active routine so we can restart it cleanly.
    private Coroutine announcementRoutine;

    private void Awake()
    {
        // If references were not assigned manually, try to find them automatically.
        if (announcementText == null)
        {
            announcementText = GetComponent<TMP_Text>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Hide the announcement at the start.
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    // Called by EnemySpawner when a new wave is about to begin.
    public void ShowWaveAnnouncement(int waveNumber, int enemiesToSpawn)
    {
        // Safety check.
        if (announcementText == null || canvasGroup == null)
        {
            return;
        }

        // Stop previous announcement if one is already running.
        if (announcementRoutine != null)
        {
            StopCoroutine(announcementRoutine);
        }

        // Set the message text.
        announcementText.text = $"WAVE {waveNumber}\n{enemiesToSpawn} ENEMIES INCOMING";

        // Start showing the announcement.
        announcementRoutine = StartCoroutine(AnnouncementRoutine());
    }

    private IEnumerator AnnouncementRoutine()
    {
        // Fade in.
        yield return FadeCanvasGroup(0f, 1f, fadeDuration);

        // Stay visible.
        yield return new WaitForSeconds(visibleDuration);

        // Fade out.
        yield return FadeCanvasGroup(1f, 0f, fadeDuration);

        // Clear routine reference.
        announcementRoutine = null;
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate fade progress from 0 to 1.
            float progress = elapsedTime / duration;

            // Smoothly interpolate alpha.
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

            yield return null;
        }

        // Make sure final value is exact.
        canvasGroup.alpha = endAlpha;
    }
}