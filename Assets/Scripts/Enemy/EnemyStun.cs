using UnityEngine;

public class EnemyStun : MonoBehaviour, IStunnable
{
    public bool IsStunned { get; private set; }

    private float timer;
    private float duration;

    public void Stun(float duration)
    {
        this.duration = duration;
        timer = 0f;
        IsStunned = true;
    }

    public void Tick(float deltaTime)
    {
        if (!IsStunned) return;

        timer += deltaTime;

        if (timer >= duration)
        {
            IsStunned = false;
        }
    }
}