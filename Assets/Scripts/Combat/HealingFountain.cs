using UnityEngine;

public class HealingFountain : MonoBehaviour, IInteractable
{
    [SerializeField] private int rechargeAmount = 3;
    [SerializeField] private string consumableNameToRecharge = "Calabaza Sanadora"; // Opcional: filtrar

    public void Interact(GameObject interactor)
    {
        bool rechargedAnything = false;

        // 1. Recargar el item que tiene equipado en la mano
        DurableItem handDurable = EquipmentManager.Instance.CurrentItemInHand?.GetComponent<DurableItem>();
        if (handDurable != null && handDurable.itemData.itemType == ItemType.Consumable)
        {
            int before = handDurable.currentUses;
            handDurable.RechargeUses(rechargeAmount);
            Debug.Log($"[Fuente] {handDurable.itemData.itemName} recargado en mano ({before} → {handDurable.currentUses})");
            rechargedAnything = true;
        }

        // 2. Recargar consumibles dentro del inventario
        // Nota: Como DurableItem solo existe en la mano, necesitamos mejorar esto en el futuro
        foreach (var slot in InventorySystem.Instance.inventory)
        {
            if (slot.item == null || slot.item.itemType != ItemType.Consumable)
                continue;

            // Por ahora solo mostramos que se "recargaría" (necesitamos guardar los usos en el inventario)
            if (slot.item.itemName.Contains(consumableNameToRecharge) ||
                slot.item.itemName.ToLower().Contains("calabaza"))
            {
                Debug.Log($"[Fuente] Se recargaría {slot.item.itemName} en inventario (pendiente de implementar)");
                rechargedAnything = true;
            }
        }

        if (rechargedAnything)
            Debug.Log($"[Fuente] ¡Recarga completada! (+{rechargeAmount} usos)");
        else
            Debug.Log("[Fuente] No se encontró ningún consumible para recargar");
    }
}