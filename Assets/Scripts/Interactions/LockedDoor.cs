using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    [Header("Configuración de la puerta")]
    [SerializeField] private string requiredKeyID = "RedKey";   // Debe coincidir con el keyID de la llave
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private bool openClockwise = true;

    [Header("Comportamiento")]
    [SerializeField] private bool consumeKey = false;   // Si true, consume la llave al abrir

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        closedRotation = transform.rotation;

        Vector3 axis = Vector3.up * (openClockwise ? 1f : -1f);
        openRotation = Quaternion.Euler(axis * openAngle) * closedRotation;
    }

    private void Update()
    {
        if (isMoving)
        {
            Quaternion target = isOpen ? openRotation : closedRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * openSpeed);

            if (Quaternion.Angle(transform.rotation, target) < 0.1f)
            {
                transform.rotation = target;
                isMoving = false;
            }
        }
    }

    public void Interact()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

        if (isOpen)
        {
            // Cerrar la puerta
            isOpen = false;
            isMoving = true;
        }
        else
        {
            // Intentar abrir
            if (inventory != null && inventory.HasKey(requiredKeyID))
            {
                isOpen = true;
                isMoving = true;

                if (consumeKey)
                    inventory.RemoveKey(requiredKeyID);

                // Aquí puedes poner sonido de abrir
            }
            else
            {
                Debug.Log("¡Necesitas la llave correcta para abrir esta puerta!");
                // Aquí más adelante puedes mostrar mensaje en pantalla
            }
        }
    }
}