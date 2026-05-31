using UnityEngine;
using UnityEngine.AI;

public class EnemyAudio : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private NavMeshAgent agent;

    [Header("Loop Source")]
    [SerializeField] private AudioSource loopSource;

    [Header("Loop IDs")]
    [SerializeField] private string chaseLoopID = "enemy_chase"; // ¡Mucho mejor nombre!

    [Header("SFX IDs")]
    [SerializeField] private string meleeAttackSFX = "enemy_melee_attack";
    [SerializeField] private string rangedAttackSFX = "enemy_ranged_attack";

    [Header("Footsteps SFX (Editable en Inspector)")]
    [SerializeField] private string[] patrolSteps = { "enemy_footstep1", "enemy_footstep2" };
    [SerializeField] private string[] chaseSteps = { "enemy_footstep1", "enemy_footstep2" };

    [Header("Footsteps Settings")]
    [SerializeField] private float patrolStepInterval = 0.55f;
    [SerializeField] private float chaseStepInterval = 0.32f;

    private float stepTimer;
    private int patrolIndex;
    private int chaseIndex;

    private void Awake()
    {
        if (loopSource != null)
        {
            loopSource.loop = true;
            loopSource.playOnAwake = false;
            loopSource.spatialBlend = 1f;
        }

        if (enemyMovement == null) enemyMovement = GetComponent<EnemyMovement>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // El Update SOLO maneja los pasos. Cero conflictos de lógica.
        HandleFootsteps();
    }

    // =====================================================
    // FOOTSTEPS (CON ALTERNANCIA SECUENCIAL)
    // =====================================================
    private void HandleFootsteps()
    {
        if (enemyMovement == null || agent == null) return;

        if (enemyMovement.IsMovingTowardsTarget() && agent.velocity.sqrMagnitude > 0.05f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();

                bool isChasing = agent.speed > 3.0f;
                stepTimer = isChasing ? chaseStepInterval : patrolStepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
            patrolIndex = 0;
            chaseIndex = 0;
        }
    }

    private void PlayFootstep()
    {
        bool isChasing = agent.speed > 3.0f;
        string selectedSFX = "";

        if (isChasing && chaseSteps.Length > 0)
        {
            selectedSFX = GetNext(chaseSteps, ref chaseIndex);
        }
        else if (!isChasing && patrolSteps.Length > 0)
        {
            selectedSFX = GetNext(patrolSteps, ref patrolIndex);
        }

        if (!string.IsNullOrEmpty(selectedSFX))
        {
            AudioManager.Instance.PlaySFX3D(selectedSFX, transform.position);
        }
    }

    private string GetNext(string[] array, ref int index)
    {
        if (index >= array.Length) index = 0;

        string clipId = array[index];
        index = (index + 1) % array.Length;
        return clipId;
    }

    // =====================================================
    // CHASE LOOP (MÉTODOS PUBLICOS PARA EL BRAIN)
    // =====================================================
    public void PlayChaseLoop()
    {
        if (loopSource == null) return;

        AudioClip clip = AudioManager.Instance.GetSFXClip(chaseLoopID);
        if (clip == null) return;

        if (loopSource.clip == clip && loopSource.isPlaying) return;

        loopSource.Stop();
        loopSource.clip = clip;
        loopSource.Play();
    }

    public void StopChaseLoop()
    {
        if (loopSource == null) return;

        if (loopSource.isPlaying)
        {
            loopSource.Stop();
        }
    }

    // =====================================================
    // ATTACKS
    // =====================================================
    public void PlayMeleeAttack()
    {
        AudioManager.Instance.PlaySFX3D(meleeAttackSFX, transform.position);
    }

    public void PlayRangedAttack()
    {
        AudioManager.Instance.PlaySFX3D(rangedAttackSFX, transform.position);
    }
}