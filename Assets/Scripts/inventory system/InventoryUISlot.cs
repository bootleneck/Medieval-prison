using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;

    public void Setup(InventorySlot slot)
    {
        icon.sprite = slot.item.icon;

        amountText.text = slot.amount > 1
            ? slot.amount.ToString()
            : "";
    }
}