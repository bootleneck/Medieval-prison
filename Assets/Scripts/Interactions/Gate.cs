using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float openHeight = 4.5f;
    [SerializeField] private float moveSpeed = 0.8f;

    [Header("Audio")]
    [SerializeField] private string gateSFX = "gate_open";

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    private bool isMoving = false;
    public bool IsMoving => isMoving;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    private void Update()
    {
        if (!isMoving) return;

        Vector3 target = isOpen ? openPosition : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            transform.position = target;
            isMoving = false;
        }
    }

    // Llamado por la palanca
    public void Open()
    {
        if (isOpen || isMoving) return;

        isOpen = true;
        isMoving = true;

        PlaySFX();
    }

    public void Close()
    {
        if (!isOpen || isMoving) return;

        isOpen = false;
        isMoving = true;

        PlaySFX();
    }

    private void PlaySFX()
    {
        if (!string.IsNullOrEmpty(gateSFX))
            AudioManager.Instance.PlaySFX3D(gateSFX, transform.position);
    }
}