using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AcidProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public float lifetime = 3f;

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

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) return;

        if (other.GetComponentInParent<IDamageable>() is IDamageable dmg)
        {
            dmg.TakeDamage(damage);
            hasHit = true;
        }

        Destroy(gameObject);
    }
}