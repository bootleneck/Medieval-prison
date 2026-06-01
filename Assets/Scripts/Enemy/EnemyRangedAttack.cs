using System.Collections;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Ranged Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 15;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject acidPrefab;

    [Header("Timing")]
    [SerializeField] private float windupTime = 0.4f;
    [SerializeField] private float recoveryTime = 0.3f;

    [Header("References")]
    [SerializeField] private Animator animator;
    private EnemyAudio enemyAudio;

    private float lastAttackTime;
    private bool isAttacking;

    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown && !isAttacking;

    private void Awake()
    {
        enemyAudio = GetComponent<EnemyAudio>();
    }

    public void StartRangedAttack()
    {
        var brain = GetComponent<EnemyBrain>();
        if (!CanAttack || brain.IsAttacking) return;

        StartCoroutine(RangedAttackRoutine());
    }

    private IEnumerator RangedAttackRoutine()
    {
        var brain = GetComponent<EnemyBrain>();
        brain.SetAttacking(true);

        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("RangedAttack");
        enemyAudio?.PlayRangedAttack();

        yield return new WaitForSeconds(windupTime);

        ShootAcid();

        yield return new WaitForSeconds(recoveryTime);

        isAttacking = false;
        brain.SetAttacking(false);
    }

    private void ShootAcid()
    {
        if (attackPoint == null || acidPrefab == null) return;

        GameObject acid = Instantiate(acidPrefab, attackPoint.position, attackPoint.rotation);

        if (acid.TryGetComponent<AcidProjectile>(out var proj))
        {
            proj.damage = damage;
            proj.lifetime = 3f;
        }

        Debug.Log("💨 Acid disparado");
    }
}