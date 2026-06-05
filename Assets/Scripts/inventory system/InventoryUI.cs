using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform content;
    public GameObject slotPrefab;

    private List<GameObject> currentSlots = new List<GameObject>();

    private int lastItemCount = -1;

    void Update()
    {
        if (InventorySystem.Instance.inventory.Count != lastItemCount)
        {
            RefreshUI();
            lastItemCount = InventorySystem.Instance.inventory.Count;
        }
    }

    void RefreshUI()
    {        
        foreach (GameObject slot in currentSlots)
        {
            Destroy(slot);
        }

        currentSlots.Clear();

        foreach (InventorySlot slotData in InventorySystem.Instance.inventory)
        {
            GameObject newSlot = Instantiate(slotPrefab, content);

            InventoryUISlot uiSlot = newSlot.GetComponent<InventoryUISlot>();
            uiSlot.Setup(slotData);

            currentSlots.Add(newSlot);
        }
    }
}