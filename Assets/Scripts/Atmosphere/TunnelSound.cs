using UnityEngine;

public class TunnelSound : MonoBehaviour
{
    [SerializeField] private string tunnelSound = "tunnelSound";
    [SerializeField] private LayerMask playerLayer;

    private bool hasPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            hasPlayed = true;

            AudioManager.Instance.PlaySFX3D(tunnelSound, transform.position);

            Destroy(gameObject);
        }
    }
}