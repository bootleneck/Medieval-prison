public abstract class EnemyState
{
    public abstract void Enter(EnemyBrain brain);
    public abstract void Update(EnemyBrain brain);
    public abstract void Exit(EnemyBrain brain);
}