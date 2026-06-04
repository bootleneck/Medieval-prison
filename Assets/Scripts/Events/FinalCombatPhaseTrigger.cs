using UnityEngine;

public class FinalCombatPhaseTrigger : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int phaseIndex;

    [SerializeField] private EnemySpawner[] spawners;

    [SerializeField] private bool destroyAfterActivation = true;

    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (!other.GetComponentInParent<CharacterController>())
            return;

        var state = FinalCombatState.Instance;

        if (state == null)
            return;

        // 🔒 control de orden
        if (!state.CanActivate(phaseIndex))
            return;

        activated = true;

        state.AdvancePhase();

        // 🎵 música solo en primera fase
        if (phaseIndex == 1)
            GameMusicController.Instance?.PlayFinalCombatMusic();

        foreach (var spawner in spawners)
        {
            if (spawner != null)
                spawner.EnableSpawner();
        }

        if (destroyAfterActivation)
            Destroy(gameObject);
    }
}