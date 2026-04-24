using UnityEngine;
using UnityEngine.Audio;

public class DropletSpawn : MonoBehaviour
{
    [SerializeField] GameObject dropletPrefab;
    [SerializeField] Transform pointSpawn;
    [SerializeField] float minInterval = 1.5f;
    [SerializeField] float maxInterval = 4f;

    float interval;

    private float timer;
    void Start()
    {
        interval = Random.Range(minInterval, maxInterval);
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            CreateDoplet();
            timer = 0f;

            interval = Random.Range(minInterval, maxInterval); // nuevo intervalo aleatorio
        }
    }

    void CreateDoplet()
    {
        GameObject droplet = Instantiate(dropletPrefab, pointSpawn.position, Quaternion.identity);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Puddle"))
        {
             Destroy(gameObject);
        }
    }
}
