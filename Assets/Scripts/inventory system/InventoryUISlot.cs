using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    // Cambiamos a 'public' o '[SerializeField]' para que no dependa solo de transform.Find
    public Image icon;
    public TMP_Text amountText;

    private void Awake()
    {
        // Si no los asignaste en el inspector, los busca, pero de forma segura
        if (icon == null)
        {
            Transform iconTransform = transform.Find("Icon");
            if (iconTransform != null) icon = iconTransform.GetComponent<Image>();
        }

        if (amountText == null)
        {
            Transform textTransform = transform.Find("AmountText");
            if (textTransform != null) amountText = textTransform.GetComponent<TMP_Text>();
        }
    }

    public void Setup(InventorySlot slot)
    {
        // REVISIÓN DE SEGURIDAD: Si por alguna razón Awake no llegó a tiempo, buscamos de nuevo
        if (icon == null) icon = transform.Find("Icon")?.GetComponent<Image>();

        if (slot.item != null && icon != null)
        {
            // ASIGNAR ICONO DEL ITEM
            icon.sprite = slot.item.icon;

            // ASEGURAR VISIBILIDAD: Forzamos color blanco y activamos la imagen
            icon.color = Color.white;
            icon.enabled = true;
        }

        if (amountText != null)
        {
            // MOSTRAR CANTIDAD
            amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
        }
    }
}