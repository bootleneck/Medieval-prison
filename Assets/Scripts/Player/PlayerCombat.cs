using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stun Attack")]
    [SerializeField] private float stunCost = 35f;
    [SerializeField] private float stunRange = 2f;
    [SerializeField] private float stunDuration = 5f;

    [Header("Combat")]
    [SerializeField] private LayerMask hitLayers;

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

    // ================= INPUT =================

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking)
            return;

        ItemData equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;

        isAttacking = true;
        animator.SetTrigger("SlashAttack");
    }

    public void OnStunAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking)
            return;

        ItemData equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null || equipped.itemType != ItemType.Weapon) return;
        if (!stamina.HasStamina(stunCost)) return;

        isAttacking = true;
        animator.SetTrigger("StunAttack");
    }

    // ================= SLASH =================

    public void DealSlashDamage()
    {
        ItemData equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, equipped.range, hitLayers);

        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponentInParent<IDamageable>();
            Structure structure = hit.GetComponentInParent<Structure>();

            // ESPADA → SOLO enemigos
            if (equipped.itemType == ItemType.Weapon)
            {
                if (structure != null) continue;
                if (dmg != null) dmg.TakeDamage(equipped.damage);
            }
            // MALLET → SOLO estructuras
            else if (equipped.itemType == ItemType.Tool)
            {
                if (structure == null) continue;

                GateBreak gate = hit.GetComponentInParent<GateBreak>();
                if (gate != null)
                {
                    gate.SetPlayerPosition(transform.position);
                    gate.AddHitForce(); // empuje por golpe
                }

                // Aplica daño solo si tiene Health
                if (dmg != null)
                {
                    dmg.TakeDamage(equipped.damage);
                }
            }
        }
    }

    // ================= STUN =================

    public void DealStunAttack()
    {
        ItemData equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null || equipped.itemType != ItemType.Weapon) return;

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, stunRange, hitLayers);

        foreach (var hit in hits)
        {
            IStunnable stun = hit.GetComponentInParent<IStunnable>();
            if (stun != null)
            {
                stun.Stun(stunDuration);
                stamina.UseStamina(stunCost);
            }
        }
    }

    // ================= END =================

    public void EndAttack()
    {
        isAttacking = false;
    }

    // ================= DEBUG =================

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        ItemData equipped = null;
        if (EquipmentManager.Instance != null)
            equipped = EquipmentManager.Instance.currentEquippedItem;

        float range = equipped != null ? equipped.range : 2f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, stunRange);
    }
}