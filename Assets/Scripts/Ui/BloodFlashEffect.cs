using UnityEngine;
using UnityEngine.UI;

public class BloodFlashEffect : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image damageImage;

    [Header("Configuracion del Flash")]
    [SerializeField] private float flashSpeed = 4f;
    [SerializeField] private float maxAlpha = 0.5f;
    [SerializeField] private float durationTime = 0.5f;

    private float _timer;

    private void Start()
    {
        if (playerHealth == null || damageImage == null)
        {
            Debug.LogError("[BloodFlashEffect] Faltan asignar referencias.");
            return;
        }

        playerHealth.OnDamageTaken += ActivarFlashColor;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamageTaken -= ActivarFlashColor;
        }
    }

    private void Update()
    {        
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            damageImage.color = new Color(1f, 1f, 1f, maxAlpha);
        }
        else
        {            
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    private void ActivarFlashColor(int dañoRecibido)
    {        
        _timer = durationTime;
    }
}