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

        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;

        if (equipped.itemType == ItemType.Consumable)
        {
            UseConsumable();
        }
        else
        {
            isAttacking = true;
            animator.SetTrigger("SlashAttack");
        }
    }

    public void OnStunAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking)
            return;

        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null || equipped.itemType != ItemType.Weapon) return;
        if (!stamina.HasStamina(stunCost)) return;

        isAttacking = true;
        animator.SetTrigger("StunAttack");
    }

    // ================= SLASH =================

    public void DealSlashDamage()
    {
        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null)
        {
            Debug.LogWarning("No hay item equipado para atacar");
            return;
        }

        float attackRange = equipped.range > 0 ? equipped.range : 2f;
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, hitLayers);

        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponentInParent<IDamageable>();
            Structure structure = hit.GetComponentInParent<Structure>();
            GateBreak gate = hit.GetComponentInParent<GateBreak>();

            if (equipped.itemType == ItemType.Weapon)
            {
                if (structure != null) continue; // No dañar estructuras con arma
                if (dmg != null) dmg.TakeDamage(equipped.damage);
            }
            else if (equipped.itemType == ItemType.Tool)
            {
                // === LÓGICA PARA MALLET ===
                if (gate != null)
                {
                    gate.SetPlayerPosition(transform.position);
                    gate.AddHitForce();
                }

                if (dmg != null)
                    dmg.TakeDamage(equipped.damage);

                Debug.Log($"Tool usado contra estructura: {equipped.itemName} | Daño: {equipped.damage}");
            }
        }
    }

    // ================= STUN =================

    public void DealStunAttack()
    {
        var equipped = EquipmentManager.Instance.currentEquippedItem;
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

    // ================= CONSUMABLE =================

    private void UseConsumable()
    {
        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null || equipped.itemType != ItemType.Consumable)
            return;

        var handItem = EquipmentManager.Instance.CurrentItemInHand;
        DurableItem durable = handItem?.GetComponent<DurableItem>();

        if (durable == null || !durable.Use())
        {
            Debug.Log("No se pudo usar el consumible");
            return;
        }

        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.Heal(equipped.healAmount);
        }

        if (durable.currentUses <= 0)
        {
            EquipmentManager.Instance.Equip(null); // desequipar al agotarse
        }
    }

    // Corutina para desequipar después de usar el último
    private System.Collections.IEnumerator AutoUnequipAfterUse()
    {
        yield return new WaitForSeconds(0.3f);
        EquipmentManager.Instance.Equip(null); // desequipa
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

        var equipped = EquipmentManager.Instance.currentEquippedItem;
        float range = equipped != null ? equipped.range : 2f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, stunRange);
    }
}