using UnityEngine;

public class PlayerCombatActions : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PlayerStamina stamina;
    [SerializeField] private LayerMask hitLayers;

    [Header("Stun")]
    [SerializeField] private float stunCost = 35f;
    [SerializeField] private float stunRange = 2f;
    [SerializeField] private float stunDuration = 5f;

    private void Awake()
    {
        stamina = GetComponent<PlayerStamina>();
    }

    public void DealSlashDamage()
    {
        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;

        float range = equipped.range > 0 ? equipped.range : 2f;

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, range, hitLayers);

        foreach (var hit in hits)
        {
            IHitReaction reaction = hit.GetComponentInParent<IHitReaction>();
            reaction?.Hit(equipped, transform.root.position);

            if (equipped.itemType == ItemType.Weapon)
            {
                IDamageable dmg = hit.GetComponentInParent<IDamageable>();
                dmg?.TakeDamage(equipped.damage);
            }
        }
    }

    public void DealStunAttack()
    {
        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;
        if (equipped.itemType != ItemType.Weapon) return; // Solo espada hace stun

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

    public void UseConsumable()
    {
        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;

        var handItem = EquipmentManager.Instance.CurrentItemInHand;
        DurableItem durable = handItem?.GetComponent<DurableItem>();

        if (durable == null || !durable.Use())
        {
            Debug.Log("No se pudo usar el consumible");
            return;
        }

        Health health = GetComponent<Health>();
        health?.Heal(equipped.healAmount);

        if (durable.currentUses <= 0)
            EquipmentManager.Instance.Equip(null);
    }
}