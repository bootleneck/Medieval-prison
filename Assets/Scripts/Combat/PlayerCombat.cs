using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stun Attack")]
    [SerializeField] private float _stunCost = 35f;
    [SerializeField] private float _stunRange = 2f;
    [SerializeField] private float _attackDelay = 0.25f; // tiempo hasta el impacto

    private PlayerStamina _stamina;
    private Animator animator;

    private void Awake()
    {
        _stamina = GetComponent<PlayerStamina>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.ResetTrigger("StunAttack");
            animator.Play("Idle", 0, 0f); // asegura que arranca en Idle
        }
    }

    // IMPORTANTE: se llama OnAttack porque tu Input Action es "Attack"
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || _stamina == null)
            return;

        if (!_stamina.HasStamina(_stunCost))
            return;

        // 🔥 dispara animación primero
        if (animator != null)
            animator.SetTrigger("StunAttack");

        // 🔥 ejecuta el golpe con delay para sincronizar con animación
        StartCoroutine(StunAttackRoutine());
    }

    private IEnumerator StunAttackRoutine()
    {
        // Espera el momento del golpe
        yield return new WaitForSeconds(_attackDelay);

        PerformStunAttack();
    }

    private void PerformStunAttack()
    {
        Debug.Log("STUN ATTACK");

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _stunRange))
        {
            // IMPORTANTE: por si el collider está en un hijo
            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                enemy.Stun();

                // Usa stamina solo si golpea
                _stamina.UseStamina(_stunCost);

                // 🔥 Opcional: aquí podrías añadir sonido, hitstop o cámara shake
            }
        }
    }
}