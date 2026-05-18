using UnityEngine;

public class EnemyStun : MonoBehaviour
{
    public bool IsStunned { get; private set; }
    private float stunTimer;

    public void Tick(float deltaTime)
    {
        if (!IsStunned) return;

        stunTimer -= deltaTime;
        if (stunTimer <= 0)
        {
            IsStunned = false;
            Debug.Log($"[EnemyStun] {gameObject.name} ya no está aturdido.");
        }
    }

    public void ApplyStun(float duration)
    {
        IsStunned = true;
        stunTimer = duration;
        Debug.Log($"[EnemyStun] {gameObject.name} aturdido por {duration} segundos.");
    }
}