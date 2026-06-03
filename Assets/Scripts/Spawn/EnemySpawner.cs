using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Referencias del Almacén")]
    // Asignamos el Pool específico para este punto de spawn
    [SerializeField] private EnemyPool enemyPool;

    [Header("Tiempos de Spawn")]
    [SerializeField] private float spawnRate = 5f;
    [SerializeField] private float spawnDelay = 2f;

    [Header("Límites del Spawner")]
    [SerializeField] private float spawnRadius = 3f;

    void Start()
    {
        if (enemyPool == null)
        {
            Debug.LogError($"[EnemySpawner] {gameObject.name} no tiene un EnemyPool asignado en el Inspector.");
            return;
        }

        InvokeRepeating(nameof(SpawnEnemyFromPool), spawnDelay, spawnRate);
    }

    private void SpawnEnemyFromPool()
    {
        if (enemyPool == null) return;

        // Pedimos el enemigo directamente a su pool asignado
        GameObject enemy = enemyPool.GetEnemy();

        if (enemy != null)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                transform.position.x + randomCircle.x,
                transform.position.y,
                transform.position.z + randomCircle.y
            );

            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            // Sincronización obligatoria si usas NavMeshAgent para IA
            var agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.Warp(spawnPosition);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}