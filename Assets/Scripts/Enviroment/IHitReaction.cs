using UnityEngine;

public interface IHitReaction
{   
    void Hit(ItemData weapon, Vector3 playerPosition);
}