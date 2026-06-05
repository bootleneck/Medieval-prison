using UnityEngine;
using System.Collections;

public class Web : MonoBehaviour, IHitReaction
{
    [Header("Audio")]
    [SerializeField] private string breakSound = "web_break";

    [Header("Break Effect")]
    [SerializeField] private float destroyDelay = 0.4f;

    private bool isBreaking;
    private Vector3 originalScale;
    private Collider webCollider;

    private void Awake()
    {
        originalScale = transform.localScale;
        
        webCollider = GetComponent<Collider>();
    }

    public void Hit(ItemData weapon, Vector3 playerPosition)
    {
        if (weapon == null || isBreaking) return;

        if (weapon.itemType == ItemType.Weapon)
        {
            StartCoroutine(BreakRoutine());
        }
    }

    private IEnumerator BreakRoutine()
    {
        isBreaking = true;
                
        if (webCollider != null)
        {
            webCollider.enabled = false;
        }
        
        AudioManager.Instance.PlaySFX3D(breakSound, transform.position);
        
        Vector3 startScale = transform.localScale;
        float t = 0f;

        while (t < destroyDelay)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / destroyDelay);
            yield return null;
        }
        
        gameObject.SetActive(false);

        isBreaking = false;
    }
    
    private void OnEnable()
    {
        if (originalScale != Vector3.zero)
        {
            transform.localScale = originalScale;
        }
        if (webCollider != null)
        {
            webCollider.enabled = true;
        }
    }
}