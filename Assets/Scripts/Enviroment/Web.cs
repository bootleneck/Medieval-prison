using UnityEngine;

public class Web : MonoBehaviour, IHitReaction
{
    public void Hit(ItemData weapon, Vector3 playerPosition)
    {
        if (weapon == null) return;

        if (weapon.itemType == ItemType.Weapon)
        {
            Debug.Log("Telaraña destruida");
            Destroy(gameObject);
        }
    }
}