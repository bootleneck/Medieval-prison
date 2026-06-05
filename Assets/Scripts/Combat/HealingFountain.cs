using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HealingFountain : MonoBehaviour, IInteractable
{
    [Header("Uso")]
    [SerializeField] private bool hasBeenUsed = false;

    [Header("Audio 3D")]
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private string interactSFXID = "fountain_repair";
    [SerializeField] private string emptySFXID = "empty_fountain";

    [SerializeField] private GameObject[] waterObjects;

    private void Awake()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;
    }

    public void Interact(GameObject interactor)
    {
        if (hasBeenUsed)
        {
            AudioManager.Instance.PlaySFX3D(emptySFXID, transform.position);
            Debug.Log("[Fuente] Esta fuente ya ha sido usada.");
            return;
        }

        bool rechargedAnything = false;
        
        DurableItem handDurable = EquipmentManager.Instance.CurrentItemInHand?.GetComponent<DurableItem>();
        if (handDurable != null)
        {
            handDurable.RechargeToFull();
            rechargedAnything = true;
        }
        
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

            AudioManager.Instance.PlaySFX3D(interactSFXID, transform.position);

            foreach (GameObject water in waterObjects)
            {
                if (water != null)
                    water.SetActive(false);
            }            
            if (ambientSource != null)
            {
                ambientSource.Stop();
                ambientSource.enabled = false;
            }
            Debug.Log("[Fuente] ¡Todos los items han sido recargados!");
        }
    }
}