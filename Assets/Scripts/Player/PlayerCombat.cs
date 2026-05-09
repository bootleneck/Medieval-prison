using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Slash Attack")]
    [SerializeField] private int slashDamage = 25;
    [SerializeField] private float slashRange = 2f;
    [SerializeField] private LayerMask hitLayers;

    [Header("Stun Attack")]
    [SerializeField] private float stunCost = 35f;
    [SerializeField] private float stunRange = 2f;
    [SerializeField] private float stunDuration = 5f;

    [Header("Refs")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PlayerStamina stamina;
    [SerializeField] private Animator animator;

    private bool isAttacking;

    private void Awake()
    {
        stamina = GetComponent<PlayerStamina>();
        animator = GetComponent<Animator>();
    }

    // =========================================================
    // INPUT
    // =========================================================

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking)
            return;

        isAttacking = true;
        animator.SetTrigger("SlashAttack");
    }

    public void OnStunAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking)
            return;

        if (!stamina.HasStamina(stunCost))
            return;

        isAttacking = true;
        animator.SetTrigger("StunAttack");
    }

    // =========================================================
    // SLASH (Animation Event)
    // =========================================================

    public void DealSlashDamage()
    {
        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            slashRange,
            hitLayers
        );

        foreach (var hit in hits)
        {
            IDamageable dmg =
                hit.GetComponentInParent<IDamageable>();

            if (dmg != null)
                dmg.TakeDamage(slashDamage);
        }
    }

    // =========================================================
    // STUN (Animation Event)
    // =========================================================

    public void DealStunAttack()
    {
        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            stunRange,
            hitLayers
        );

        foreach (var hit in hits)
        {
            IStunnable stun =
                hit.GetComponentInParent<IStunnable>();

            if (stun != null)
            {
                stun.Stun(stunDuration);
                stamina.UseStamina(stunCost);
            }
        }
    }

    // =========================================================
    // END
    // =========================================================

    public void EndAttack()
    {
        isAttacking = false;
    }

    // Debug visual
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, slashRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, stunRange);
    }
}