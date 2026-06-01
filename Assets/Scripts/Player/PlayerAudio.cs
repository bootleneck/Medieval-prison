using System.Collections;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("SFX IDs")]
    [SerializeField] private string painSFX = "player_pain";
    [SerializeField] private string deathSFX = "player_death";
    [SerializeField] private string heartbeatSFX = "heart_beat";

    [Header("Heartbeat Settings")]
    [SerializeField] private int heartbeatThreshold = 25;
    [SerializeField] private float heartbeatCooldown = 1.0f;

    [Header("Dependencies")]
    [SerializeField] private Health playerHealth;

    [Header("UI")]
    [SerializeField] private HeartbeatUIEffect heartbeatUI;

    private bool canPlayHeartbeat = true;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamageTaken += PlayPain;
            playerHealth.OnDamageTaken += CheckDeath;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamageTaken -= PlayPain;
            playerHealth.OnDamageTaken -= CheckDeath;
        }
    }

    private void Update()
    {
        HandleHeartbeat();
    }

    // =========================
    // DAMAGE / DEATH
    // =========================

    private void PlayPain(int damage)
    {
        if (playerHealth.CurrentHealth > 0)
        {
            AudioManager.Instance.PlaySFX3D(painSFX, transform.position);
        }
    }

    private void CheckDeath(int damage)
    {
        if (playerHealth.CurrentHealth <= 0)
        {
            AudioManager.Instance.PlaySFX3D(deathSFX, transform.position);
        }
    }

    // =========================
    // HEARTBEAT
    // =========================

    private void HandleHeartbeat()
    {
        if (playerHealth == null || playerHealth.IsDead) return;

        if (playerHealth.CurrentHealth <= heartbeatThreshold)
        {
            TryPlayHeartbeat();
        }
    }

    private void TryPlayHeartbeat()
    {
        if (!canPlayHeartbeat) return;

        AudioManager.Instance.PlaySFX3D(heartbeatSFX, transform.position);

        // UI sync
        heartbeatUI?.PlayPulse();

        StartCoroutine(HeartbeatCooldown());
    }

    private IEnumerator HeartbeatCooldown()
    {
        canPlayHeartbeat = false;
        yield return new WaitForSeconds(heartbeatCooldown);
        canPlayHeartbeat = true;
    }
}