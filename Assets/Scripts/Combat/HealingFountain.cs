using UnityEngine;

public class HealingFountain : MonoBehaviour, IInteractable
{
    [SerializeField] private bool hasBeenUsed = false;

    public void Interact(GameObject interactor)
    {
        if (hasBeenUsed)
        {
            Debug.Log("[Fuente] Esta fuente ya ha sido usada.");
            return;
        }

        bool rechargedAnything = false;

        // 1. Recargar completamente el objeto en mano
        DurableItem handDurable = EquipmentManager.Instance.CurrentItemInHand?.GetComponent<DurableItem>();
        if (handDurable != null)
        {
            int before = handDurable.currentUses;
            handDurable.RechargeToFull();
            Debug.Log($"[Fuente] {handDurable.itemData.itemName} (en mano) recargado completamente ({before} → {handDurable.currentUses})");
            rechargedAnything = true;
        }

        // 2. Recargar completamente todos los items del inventario
        foreach (var slot in InventorySystem.Instance.inventory)
        {
            if (slot.item == null) continue;

            if (slot.item.itemType == ItemType.Weapon ||
                slot.item.itemType == ItemType.Tool ||
                slot.item.itemType == ItemType.Consumable)
            {
                int before = slot.currentUses;
                int max = (slot.item.itemType == ItemType.Consumable)
                          ? slot.item.maxConsumableUses
                          : slot.item.maxUses;

                if (slot.currentUses < max)
                {
                    slot.currentUses = max;
                    Debug.Log($"[Fuente] {slot.item.itemName} (inventario) recargado completamente");
                    rechargedAnything = true;
                }
            }
        }

        if (rechargedAnything)
        {
            hasBeenUsed = true;
            Debug.Log("[Fuente] ¡Todos los items han sido recargados completamente!");
            // Opcional: Desactivar visualmente la fuente
            // gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("[Fuente] No había nada para recargar");
        }
    }
}