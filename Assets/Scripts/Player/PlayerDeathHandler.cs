using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        Debug.Log("[PlayerDeathHandler] Jugador muerto → Cargando pantalla de derrota");

        GameMusicController.Instance?.StopFinalCombatMusic();

        GameManager.instance.LoadGameOver();
    }
}