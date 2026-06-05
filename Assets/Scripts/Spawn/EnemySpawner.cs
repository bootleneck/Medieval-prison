using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private EnemyPool enemyPool;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate = 5f;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private int maxEnemiesAlive = 5;
    [SerializeField] private float spawnRadius = 3f;

    [Header("Activation")]
    [SerializeField] private bool startEnabled = true;

    private bool spawningEnabled;
    private float timer;

    private void Start()
    {
        if (startEnabled)
            EnableSpawner();
    }

    private void Update()
    {
        if (!spawningEnabled || enemyPool == null)
            return;

        timer += Time.deltaTime;

        if (timer < spawnRate)
            return;

        timer = 0f;
        TrySpawnEnemy();
    }

    public void EnableSpawner()
    {
        spawningEnabled = true;
        timer = -spawnDelay;
    }

    public void DisableSpawner()
    {
        spawningEnabled = false;
    }

    private void TrySpawnEnemy()
    {
        if (enemyPool.GetActiveCount() >= maxEnemiesAlive)
            return;

        if (!TryGetValidNavMeshPosition(out Vector3 spawnPos))
            return;

        GameObject enemy = enemyPool.GetEnemy();

        enemy.transform.position = spawnPos;
        enemy.transform.rotation = Quaternion.identity;
       
        enemy.SetActive(true);

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            // 3. Activamos el agente ahora que ya está sobre el NavMesh válido
            agent.enabled = true;
            agent.Warp(spawnPos);
        }
    }

    private bool TryGetValidNavMeshPosition(out Vector3 result)
    {
        Vector2 random = Random.insideUnitCircle * spawnRadius;
        Vector3 rawPos = transform.position + new Vector3(random.x, 0f, random.y);

        if (NavMesh.SamplePosition(rawPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}