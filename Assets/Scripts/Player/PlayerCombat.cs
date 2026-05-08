using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Slash Attack")]
    [SerializeField] private int _slashDamage = 25;
    [SerializeField] private float _slashRange = 2f;

    [Header("Stun Attack")]
    [SerializeField] private float _stunCost = 35f;
    [SerializeField] private float _stunRange = 2f;
    [SerializeField] private float _stunDuration = 5f;

    [Header("Layers")]
    [SerializeField] private LayerMask _hitLayers;

    private PlayerStamina _stamina;
    private Animator _animator;
    private Camera _cam;

    private bool _isAttacking;

    private void Awake()
    {
        _stamina = GetComponent<PlayerStamina>();
        _animator = GetComponent<Animator>();
        _cam = Camera.main;
    }

    // =========================================================
    // INPUTS
    // =========================================================

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || _isAttacking)
            return;

        _isAttacking = true;
        _animator.SetTrigger("SlashAttack");
    }

    public void OnStunAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || _isAttacking)
            return;

        if (!_stamina.HasStamina(_stunCost))
            return;

        _isAttacking = true;
        _animator.SetTrigger("StunAttack");
    }

    // =========================================================
    // SLASH DAMAGE
    // =========================================================

    public void DealSlashDamage()
    {
        Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _slashRange, _hitLayers))
        {
            var dmg = hit.collider.GetComponentInParent<IDamageable>();

            if (dmg != null)
            {
                dmg.TakeDamage(_slashDamage);
            }
        }
    }

    // =========================================================
    // STUN ATTACK
    // =========================================================

    public void DealStunAttack()
    {
        Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _stunRange, _hitLayers))
        {
            var stun = hit.collider.GetComponentInParent<IStunnable>();

            if (stun != null)
            {
                stun.Stun(_stunDuration);
                _stamina.UseStamina(_stunCost);
            }
        }
    }

    // =========================================================
    // ANIMATION END
    // =========================================================

    public void EndAttack()
    {
        _isAttacking = false;
    }
}