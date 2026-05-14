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
    [SerializeField]
    private string lockedMessage =
        "La puerta está cerrada con llave";

    [Header("Animación")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

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

        Quaternion target =
            isOpen ? targetRotation : closedRotation;

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
        // Evita spam mientras la puerta se mueve
        if (isMoving) return;

        // Si está abierta → cerrar
        if (isOpen)
        {
            CloseDoor();
            return;
        }

        // Ya desbloqueada previamente
        if (unlocked)
        {
            OpenDoor(interactor);
            return;
        }

        // Puerta normal (sin llave)
        if (!requiresKey)
        {
            OpenDoor(interactor);
            return;
        }

        // Verificar llave
        if (InventorySystem.Instance.HasKey(requiredKey))
        {
            OpenDoor(interactor);

            // Consumir llave
         /*   if (consumeKey)
            {
                InventorySystem.Instance.UseKey(requiredKey);
            }
         */
            // Desbloqueo permanente
            if (permanentlyUnlock)
            {
                unlocked = true;
            }
        }
        else
        {
            Debug.Log(lockedMessage);
        }
    }

    private void OpenDoor(GameObject interactor)
    {
        isOpen = true;
        isMoving = true;

        // Dirección del jugador respecto a la puerta
        Vector3 directionToPlayer =
            interactor.transform.position - transform.position;

        // Detecta de qué lado está el jugador
        float dot =
            Vector3.Dot(transform.right, directionToPlayer);

        // Define dirección de apertura
        float direction = dot >= 0 ? 1f : -1f;

        // Calcula rotación objetivo
        targetRotation =
            Quaternion.Euler(
                0f,
                openAngle * direction,
                0f
            ) * closedRotation;
    }

    private void CloseDoor()
    {
        isOpen = false;
        isMoving = true;
    }
}