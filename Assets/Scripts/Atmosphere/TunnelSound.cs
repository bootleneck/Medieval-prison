using UnityEngine;

public class TunnelSound : MonoBehaviour
{
    public AudioClip tunnelSound;
    public LayerMask playerLayer;
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && (playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            hasPlayed = true;
            AudioSource.PlayClipAtPoint(tunnelSound, transform.position);
            Destroy(gameObject, tunnelSound.length);
        }
    }
}
