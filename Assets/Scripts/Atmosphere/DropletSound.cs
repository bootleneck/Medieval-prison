using UnityEngine;

public class DropletSound : MonoBehaviour
{
    public AudioClip dropletSound;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Puddle"))
        {
            AudioSource.PlayClipAtPoint(dropletSound, transform.position);
            Destroy(gameObject,0.5f);
        }
    }
}
