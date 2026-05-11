using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    private Image icon;
    private TMP_Text amountText;

    private void Awake()
    {
        // BUSCAR ICON AUTOMÁTICAMENTE
        icon = transform.Find("Icon").GetComponent<Image>();

        // BUSCAR TEXTO AUTOMÁTICAMENTE
        amountText = transform.Find("AmountText").GetComponent<TMP_Text>();
    }

    public void Setup(InventorySlot slot)
    {
        // ASIGNAR ICONO DEL ITEM
        icon.sprite = slot.item.icon;

        // MOSTRAR CANTIDAD
        amountText.text = slot.amount > 1
            ? slot.amount.ToString()
            : "";
    }
}