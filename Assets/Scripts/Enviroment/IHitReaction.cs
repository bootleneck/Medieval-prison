using UnityEngine;

public interface IHitReaction
{
    // Recibe el arma y la posición del jugador que golpea
    void Hit(ItemData weapon, Vector3 playerPosition);
}