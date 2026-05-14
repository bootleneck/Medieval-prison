using UnityEngine;

public class GateBreak : MonoBehaviour, IHitReaction
{
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private float torqueForce = 3f;
    [SerializeField] private float pushOutDistance = 0.2f;
    [SerializeField] private int hitsToBreak = 5;

    private Rigidbody rb;
    private int currentHits = 0;
    private Vector3 playerPosition;
    private Vector3 accumulatedForce;
    private bool hasBeenHitThisFrame;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        hasBeenHitThisFrame = false;
    }

    public void Hit(ItemData weapon, Vector3 playerPos)
    {
        if (weapon == null) return;
        if (weapon.itemType != ItemType.Tool) return;

        if (hasBeenHitThisFrame) return;
        hasBeenHitThisFrame = true;

        playerPosition = playerPos;
        AddHitForce();

        currentHits++;
        if (currentHits >= hitsToBreak)
        {
            BreakGate();
        }
    }

    private void AddHitForce()
    {
        Vector3 dir = (transform.position - playerPosition).normalized;
        dir.y = 0.1f;

        accumulatedForce += dir * pushForce * 0.5f;
        transform.position += dir * 0.05f;
    }

    private void BreakGate()
    {
        if (rb == null) return;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Vector3 dir = (transform.position - playerPosition).normalized;
        dir.y = 0.2f;

        Vector3 finalForce = accumulatedForce + (dir * pushForce);

        transform.position += dir * pushOutDistance;

        rb.AddForce(finalForce, ForceMode.Impulse);
        rb.AddTorque(Vector3.Cross(Vector3.up, dir) * torqueForce, ForceMode.Impulse);

        Debug.Log("Reja destruida");
    }
}