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

    [Header("Attack")]
    [SerializeField] private float slashStaminaCost = 10f;

    [Header("Audio")]
    [SerializeField] private string slashHitSound = "sword_hit";
    [SerializeField] private string slashWhiffSound = "sword_whiff";
    [SerializeField] private string stunCastSound = "sword_stun_cast";
    [SerializeField] private string stunHitSound = "sword_stun_hit";

    [Header("Tool Audio")]
    [SerializeField] private string toolHitSound = "tool_hit";
    [SerializeField] private string toolWhiffSound = "tool_whiff";
    [SerializeField] private string toolNoDamageSound = "tool_no_damage";
    [SerializeField] private int hitsBeforeWarning = 3;
    [SerializeField] private float warningCooldown = 5f;

    [Header("Consumable Audio")]
    [SerializeField] private string consumeSound = "pumpkin_drink";
    [SerializeField] private string empty_pumpkin = "empty_pumpkin";

    public float SlashStaminaCost => slashStaminaCost;
    public float StunStaminaCost => stunCost;

    private int toolHitCounter = 0;
    private float lastToolWarningTime = 0f;

    private void Awake()
    {
        stamina = GetComponent<PlayerStamina>();
    }

    public void DealSlashDamage()
    {
        stamina.UseStamina(slashStaminaCost);

        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;

        bool isTool = equipped.itemType == ItemType.Tool;

        float range = equipped.range > 0 ? equipped.range : 2f;

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, range, hitLayers);
        bool validHit = false;
        bool playedToolNoDamage = false;

        foreach (var hit in hits)
        {
            IHitReaction reaction = hit.GetComponentInParent<IHitReaction>();
            reaction?.Hit(equipped, transform.root.position);

            IDamageable dmg = hit.GetComponentInParent<IDamageable>();

            if (dmg != null)
            {
                if (isTool)
                {                    
                    AudioManager.Instance.PlaySFX(toolHitSound);

                    toolHitCounter++;
                    
                    if (!playedToolNoDamage &&
                        toolHitCounter >= hitsBeforeWarning &&
                        Time.time > lastToolWarningTime + warningCooldown)
                    {
                        AudioManager.Instance.PlaySFX(toolNoDamageSound);
                        lastToolWarningTime = Time.time;
                        playedToolNoDamage = true;
                        toolHitCounter = 0;
                    }

                    continue;
                }
                               
                dmg.TakeDamage(equipped.damage);
                validHit = true;
            }
        }
       
        if (!validHit)
        {
            if (isTool)
                AudioManager.Instance.PlaySFX(toolWhiffSound);
            else
                AudioManager.Instance.PlaySFX(slashWhiffSound);
        }
        else if (!isTool)
        {
            AudioManager.Instance.PlaySFX(slashHitSound);
            ConsumeDurability();
        }
    }

    public void DealStunAttack()
    {
        stamina.UseStamina(stunCost);

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
            AudioManager.Instance.PlaySFX(empty_pumpkin); 
            Debug.Log("No se pudo usar el consumible");
            return;
        }

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