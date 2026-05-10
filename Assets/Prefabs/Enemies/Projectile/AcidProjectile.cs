using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 15;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            // destruir al impactar con cualquier cosa que no sea la capa Enemy
            Destroy(gameObject);
        }
    }
}