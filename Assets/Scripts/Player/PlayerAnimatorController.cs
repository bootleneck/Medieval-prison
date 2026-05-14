using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void TriggerSlashAttack()
    {
        animator.SetTrigger("SlashAttack");
    }

    public void TriggerStunAttack()
    {
        animator.SetTrigger("StunAttack");
    }
}