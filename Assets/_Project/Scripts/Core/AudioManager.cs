using UnityEngine;

// This script manages simple gameplay sound effects.
// Other scripts can call it to play sounds without needing their own AudioSource.
public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]

    // AudioSource used to play one-shot sound effects.
    [SerializeField] private AudioSource audioSource;

    [Header("Sound Effects")]

    // Sound played when the player shoots.
    [SerializeField] private AudioClip shootClip;

    // Sound played when something receives damage.
    [SerializeField] private AudioClip hitClip;

    // Sound played when the player collects a pickup.
    [SerializeField] private AudioClip pickupClip;

    // Sound played when the game ends.
    [SerializeField] private AudioClip gameOverClip;

    // Static instance so other scripts can access the AudioManager easily.
    // This is acceptable for a small project, but for larger games we would use a cleaner service/event system.
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // If another AudioManager already exists, destroy this one to avoid duplicates.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Store this AudioManager as the active instance.
        Instance = this;

        // If no AudioSource was assigned manually, try to get one from the same GameObject.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Plays the shooting sound.
    public void PlayShoot()
    {
        PlaySound(shootClip);
    }

    // Plays the hit/damage sound.
    public void PlayHit()
    {
        PlaySound(hitClip);
    }

    // Plays the pickup collection sound.
    public void PlayPickup()
    {
        PlaySound(pickupClip);
    }

    // Plays the game over sound.
    public void PlayGameOver()
    {
        PlaySound(gameOverClip);
    }

    private void PlaySound(AudioClip clip)
    {
        // Safety check: if no clip or AudioSource is assigned, do nothing.
        if (clip == null || audioSource == null)
        {
            return;
        }

        // PlayOneShot allows multiple sound effects to overlap.
        // This is useful for rapid shooting or multiple hits.
        audioSource.PlayOneShot(clip);
    }
}