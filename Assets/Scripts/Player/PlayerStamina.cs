using UnityEngine;
using System.Collections;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _regenRate = 15f;
    [SerializeField] private float _regenDelay = 1.5f;

    [Header("Fatigue SFX")]
    [SerializeField] private float _sighCooldown = 2.2f;
    [SerializeField] private float _fatiguedThreshold = 25f;

    public float CurrentStamina { get; private set; }

    private float _regenTimer;
    private bool _canPlaySigh = true;

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
        if (amount <= 0f) return;

        CurrentStamina -= amount;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, _maxStamina);

        _regenTimer = _regenDelay;

        TryPlaySigh();
    }

    private void TryPlaySigh()
    {
        if (!_canPlaySigh) return;
        if (CurrentStamina > _fatiguedThreshold) return;

        AudioManager.Instance.PlaySFX("player_sigh");

        StartCoroutine(SighCooldown());
    }

    private IEnumerator SighCooldown()
    {
        _canPlaySigh = false;

        yield return new WaitForSeconds(_sighCooldown);

        _canPlaySigh = true;
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