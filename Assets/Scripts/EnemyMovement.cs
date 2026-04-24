using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    private float startX;
    private int direction = 1;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        
        if (transform.position.x >= startX + distance && direction == 1)
        {
            direction = -1;
            Flip();
        }
        
        else if (transform.position.x <= startX - distance && direction == -1)
        {
            direction = 1;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}