using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Sistema de llave")]
    [SerializeField] private bool requiresKey = false;
    [SerializeField] private ItemData requiredKey;

    [Header("Comportamiento")]
    [SerializeField] private bool consumeKey = false;
    [SerializeField] private bool permanentlyUnlock = true;

    [Header("Mensajes")]
    [SerializeField] private string lockedMessage = "La puerta está cerrada con llave";

    [Header("Animación")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private DoorAudioProfile audioProfile;

    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private bool isOpen = false;
    private bool isMoving = false;
    private bool unlocked = false;

    private void Start()
    {
        closedRotation = transform.rotation;
    }

    private void Update()
    {
        if (!isMoving) return;

        Quaternion target = isOpen ? targetRotation : closedRotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            Time.deltaTime * openSpeed
        );

        if (Quaternion.Angle(transform.rotation, target) < 0.1f)
        {
            transform.rotation = target;
            isMoving = false;
        }
    }

    public void Interact(GameObject interactor)
    {
        if (isMoving) return;

        // Cerrar si está abierta
        if (isOpen)
        {
            CloseDoor();
            return;
        }

        // Ya desbloqueada
        if (unlocked)
        {
            OpenDoor(interactor);
            return;
        }

        // Puerta libre
        if (!requiresKey)
        {
            OpenDoor(interactor);
            return;
        }

        // Tiene llave
        if (InventorySystem.Instance.HasKey(requiredKey))
        {
            OpenDoor(interactor);

            if (permanentlyUnlock)
                unlocked = true;
        }
        else
        {
            // 🔊 sonido de puerta bloqueada
            if (audioProfile != null && !string.IsNullOrEmpty(audioProfile.doorLocked))
            {
                AudioManager.Instance.PlaySFX3D(audioProfile.doorLocked, transform.position);
            }

            Debug.Log(lockedMessage);
        }
    }

    private void OpenDoor(GameObject interactor)
    {
        isOpen = true;
        isMoving = true;

        // 🔊 sonido abrir
        if (audioProfile != null && !string.IsNullOrEmpty(audioProfile.doorOpen))
        {
            AudioManager.Instance.PlaySFX3D(audioProfile.doorOpen, transform.position);
        }

        Vector3 directionToPlayer = interactor.transform.position - transform.position;

        float dot = Vector3.Dot(transform.right, directionToPlayer);

        float direction = dot >= 0 ? 1f : -1f;

        targetRotation = Quaternion.Euler(
            0f,
            openAngle * direction,
            0f
        ) * closedRotation;
    }

    private void CloseDoor()
    {
        isOpen = false;
        isMoving = true;

        // 🔊 sonido cerrar
        if (audioProfile != null && !string.IsNullOrEmpty(audioProfile.doorClose))
        {
            AudioManager.Instance.PlaySFX3D(audioProfile.doorClose, transform.position);
        }
    }
}