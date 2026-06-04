using UnityEngine;
using System.Collections.Generic;

public class Gate : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float openHeight = 4.5f;
    [SerializeField] private float moveSpeed = 0.8f;

    [Header("Audio")]
    [SerializeField] private string gateSFX = "gate_open";

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen;
    private bool isMoving;

    public bool IsMoving => isMoving;
    public bool IsOpen => isOpen;

    // Lista de palancas que controlan esta puerta
    private readonly List<SharedLever> linkedLevers = new();

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    private void Update()
    {
        if (!isMoving) return;

        Vector3 target = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            transform.position = target;
            isMoving = false;
        }
    }

    // Método para registrar palancas
    public void RegisterLever(SharedLever lever)
    {
        if (!linkedLevers.Contains(lever))
            linkedLevers.Add(lever);
    }

    // Método para desregistrar palancas si se destruyen
    public void UnregisterLever(SharedLever lever)
    {
        linkedLevers.Remove(lever);
    }

    public void Open()
    {
        if (isOpen || isMoving) return;
        isOpen = true;
        isMoving = true;
        PlaySFX();
        NotifyLevers();
    }

    public void Close()
    {
        if (!isOpen || isMoving) return;
        isOpen = false;
        isMoving = true;
        PlaySFX();
        NotifyLevers();
    }

    public void Toggle()
    {
        if (isMoving) return;
        if (isOpen) Close(); else Open();
    }

    private void NotifyLevers()
    {
        foreach (SharedLever lever in linkedLevers)
        {
            if (lever != null)
                lever.RefreshAnimation();
        }
    }

    private void PlaySFX()
    {
        if (!string.IsNullOrEmpty(gateSFX))
            AudioManager.Instance.PlaySFX3D(gateSFX, transform.position);
    }
}