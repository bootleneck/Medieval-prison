using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerCombatActions combatActions;
    [SerializeField] private PlayerAnimatorController animatorController;
    [SerializeField] private PlayerStamina stamina;

    private bool isAttacking;

    private void Awake()
    {
        if (animatorController == null)
            animatorController = GetComponent<PlayerAnimatorController>();

        if (combatActions == null)
            combatActions = GetComponent<PlayerCombatActions>();

        if (stamina == null)
            stamina = GetComponent<PlayerStamina>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking) return;

        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;

        if (equipped.itemType == ItemType.Consumable)
        {
            combatActions.UseConsumable();
            return;
        }

        if (!stamina.HasStamina(combatActions.SlashStaminaCost))
        {
            Debug.Log("No hay stamina suficiente para atacar");
            return;
        }

        isAttacking = true;
        animatorController.TriggerSlashAttack();
    }

    public void OnStunAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking) return;

        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;
        if (equipped.itemType != ItemType.Weapon) return;

        if (!stamina.HasStamina(combatActions.StunStaminaCost))
        {
            Debug.Log("No hay stamina suficiente para stun");
            return;
        }

        isAttacking = true;
        animatorController.TriggerStunAttack();
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}