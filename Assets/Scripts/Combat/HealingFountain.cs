using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HealingFountain : MonoBehaviour, IInteractable
{
    [Header("Uso")]
    [SerializeField] private bool hasBeenUsed = false;

    [Header("Audio 3D")]
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private string interactSFXID = "fountain_repair";

    private void Awake()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;        
    }

    public void Interact(GameObject interactor)
    {
        if (hasBeenUsed)
        {
            Debug.Log("[Fuente] Esta fuente ya ha sido usada.");
            return;
        }

        bool rechargedAnything = false;

        // Item en mano
        DurableItem handDurable =
            EquipmentManager.Instance.CurrentItemInHand?.GetComponent<DurableItem>();

        if (handDurable != null)
        {
            handDurable.RechargeToFull();
            rechargedAnything = true;
        }

        // Inventario
        foreach (var slot in InventorySystem.Instance.inventory)
        {
            if (slot.item == null) continue;

            if (slot.item.itemType == ItemType.Weapon ||
                slot.item.itemType == ItemType.Tool ||
                slot.item.itemType == ItemType.Consumable)
            {
                int max = (slot.item.itemType == ItemType.Consumable)
                    ? slot.item.maxConsumableUses
                    : slot.item.maxUses;

                if (slot.currentUses < max)
                {
                    slot.currentUses = max;
                    rechargedAnything = true;
                }
            }
        }

        if (rechargedAnything)
        {
            hasBeenUsed = true;

            // 🔊 SFX 3D desde AudioManager (evento puntual)
            AudioManager.Instance.PlaySFX3D(interactSFXID, transform.position);

            Debug.Log("[Fuente] ¡Todos los items han sido recargados!");
        }
    }
   
}