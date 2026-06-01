using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AcidProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 5;
    public float lifetime = 3f;
    public float slowDuration = 3f;  // duración del ácido

    private Rigidbody rb;
    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // ignorar enemigos
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) return;

        // daño normal
        if (other.GetComponentInParent<IDamageable>() is IDamageable dmg)
        {
            dmg.TakeDamage(damage);
            hasHit = true;
        }

        // efecto ácido en player
        if (other.GetComponentInParent<StickyAcidEffect>() is StickyAcidEffect acid)
        {
            acid.ApplyStickyAcid();
            hasHit = true;
        }

        Destroy(gameObject);
    }
}