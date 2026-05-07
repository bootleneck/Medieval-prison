using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _regenRate = 15f;
    [SerializeField] private float _regenDelay = 1.5f;

    public float CurrentStamina { get; private set; }

    private float _regenTimer;

    private void Awake()
    {
        CurrentStamina = _maxStamina;
    }

    private void Update()
    {
        HandleRegeneration();
    }

    public bool HasStamina(float amount)
    {
        return CurrentStamina >= amount;
    }

    public void UseStamina(float amount)
    {
        CurrentStamina -= amount;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, _maxStamina);

        _regenTimer = _regenDelay;
    }

    private void HandleRegeneration()
    {
        if (_regenTimer > 0f)
        {
            _regenTimer -= Time.deltaTime;
            return;
        }

        if (CurrentStamina < _maxStamina)
        {
            CurrentStamina += _regenRate * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, _maxStamina);
        }
    }
}