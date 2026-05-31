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

    [Header("Audio")]
    [SerializeField] private string slashHitSound = "sword_hit";
    [SerializeField] private string slashWhiffSound = "sword_whiff";
    [SerializeField] private string stunCastSound = "sword_stun_cast";
    [SerializeField] private string stunHitSound = "sword_stun_hit";

    [Header("Consumible Audio")]
    [SerializeField] private string consumeSound = "pumpkin_drink";

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
        bool validHit = false;

        foreach (var hit in hits)
        {
            IHitReaction reaction = hit.GetComponentInParent<IHitReaction>();
            reaction?.Hit(equipped, transform.root.position);

            IDamageable dmg = hit.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(equipped.damage);
                validHit = true;
            }
        }

        if (validHit)
        {
            AudioManager.Instance.PlaySFX(slashHitSound);
            ConsumeDurability();
        }
        else
        {
            AudioManager.Instance.PlaySFX(slashWhiffSound);
        }
    }

    public void DealStunAttack()
    {
        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null || equipped.itemType != ItemType.Weapon) return;

        AudioManager.Instance.PlaySFX(stunCastSound);

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, stunRange, hitLayers);
        bool hitSomething = false;

        foreach (var hit in hits)
        {
            IStunnable stun = hit.GetComponentInParent<IStunnable>();
            if (stun != null)
            {
                stun.Stun(stunDuration);
                stamina.UseStamina(stunCost);
                hitSomething = true;
            }
        }

        if (hitSomething)
        {
            AudioManager.Instance.PlaySFX(stunHitSound);
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

        // 🔊 SONIDO DE CONSUMO (CALABAZA / HEAL)
        AudioManager.Instance.PlaySFX(consumeSound);

        Health health = GetComponent<Health>();
        health?.Heal(equipped.healAmount);

        if (durable.currentUses <= 0)
            EquipmentManager.Instance.Equip(null);
    }

    private void ConsumeDurability()
    {
        var handItem = EquipmentManager.Instance.CurrentItemInHand;
        DurableItem durable = handItem?.GetComponent<DurableItem>();
        durable?.Use();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, 2f);
    }
}