using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{    
    public Image icon;
    public TMP_Text amountText;

    private void Awake()
    {        
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
        if (icon == null) icon = transform.Find("Icon")?.GetComponent<Image>();

        if (slot.item != null && icon != null)
        {            
            icon.sprite = slot.item.icon;
            
            icon.color = Color.white;
            icon.enabled = true;
        }

        if (amountText != null)
        {            
            amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
        }
    }
}