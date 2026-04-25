using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 1f;
    public float stopDistance = 1.5f; //

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > stopDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    speed * Time.deltaTime
                );
            }
        }
    }
}