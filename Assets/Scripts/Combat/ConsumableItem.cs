using UnityEngine;

public class ConsumableItem : MonoBehaviour
{
    public ItemData itemData;
    private int currentUses;

    private void Awake()
    {
        currentUses = 1; // Por default 1 uso por pickup
    }

    // Retorna true si se pudo usar
    public bool Use(GameObject user)
    {
        if (currentUses <= 0) return false;

        Health health = user.GetComponent<Health>();
        if (health != null)
        {
            health.Heal(itemData.damage); // puedes usar healAmount si agregas ese campo
            currentUses--;
            Debug.Log($"{itemData.itemName} usado. Usos restantes: {currentUses}");
            return true;
        }

        return false;
    }
}