using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform content;
    public GameObject slotPrefab;

    private void Update()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        // BORRAR SLOTS VIEJOS
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // CREAR NUEVOS
        foreach (InventorySlot slot in InventorySystem.Instance.inventory)
        {
            GameObject obj = Instantiate(slotPrefab, content);

            InventoryUISlot uiSlot = obj.GetComponent<InventoryUISlot>();
            uiSlot.Setup(slot);
        }
    }
}