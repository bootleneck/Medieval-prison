using UnityEngine;

public class Web : MonoBehaviour, IHitReaction
{
    [Header("Audio")]
    [SerializeField] private string breakSound = "web_break";

    public void Hit(ItemData weapon, Vector3 playerPosition)
    {
        if (weapon == null) return;

        if (weapon.itemType == ItemType.Weapon)
        {
            // 🔊 sonido de ruptura
            AudioManager.Instance.PlaySFX3D(breakSound, transform.position);

            Debug.Log("Telaraña destruida");

            Destroy(gameObject);
        }
    }
}