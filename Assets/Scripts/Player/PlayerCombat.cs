using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerCombatActions combatActions;
    [SerializeField] private PlayerAnimatorController animatorController;

    private bool isAttacking;

    private void Awake()
    {
        if (animatorController == null)
            animatorController = GetComponent<PlayerAnimatorController>();
        if (combatActions == null)
            combatActions = GetComponent<PlayerCombatActions>();
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

        isAttacking = true;
        animatorController.TriggerSlashAttack(); // ✅ Método público
    }

    public void OnStunAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking) return;

        var equipped = EquipmentManager.Instance.currentEquippedItem;
        if (equipped == null) return;
        if (equipped.itemType != ItemType.Weapon) return; // Solo espada

        isAttacking = true;
        animatorController.TriggerStunAttack(); // ✅ Método público
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}