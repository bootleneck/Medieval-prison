using UnityEngine;

public class GateBreak : MonoBehaviour
{
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private float torqueForce = 3f;
    [SerializeField] private float pushOutDistance = 0.2f;

    private Health health;
    private Rigidbody rb;

    private Vector3 playerPosition;
    private Vector3 accumulatedForce;

    private void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        health.OnDeath += BreakGate;
    }

    private void OnDisable()
    {
        health.OnDeath -= BreakGate;
    }

    // Player le pasa su posición al golpear
    public void SetPlayerPosition(Vector3 pos)
    {
        playerPosition = pos;
    }

    // Cada golpe suma empuje
    public void AddHitForce()
    {
        Vector3 dir = (transform.position - playerPosition).normalized;
        dir.y = 0.1f; // leve lift para física más natural

        accumulatedForce += dir * pushForce * 0.5f;

        // Pequeño empuje inmediato para que no se quede pegada
        transform.position += dir * 0.05f;
    }

    private void BreakGate()
    {
        if (rb == null)
            return;

        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 dir = (transform.position - playerPosition).normalized;
        dir.y = 0.2f;

        // Empuje acumulado + impulso final
        Vector3 finalForce = accumulatedForce + (dir * pushForce);

        // Sacarla ligeramente de la pared
        transform.position += dir * pushOutDistance;

        rb.AddForce(finalForce, ForceMode.Impulse);
        rb.AddTorque(Vector3.Cross(Vector3.up, dir) * torqueForce, ForceMode.Impulse);

        Debug.Log("Reja destruida");
    }
}