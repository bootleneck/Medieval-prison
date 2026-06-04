using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Referencias del Almacén")]
    [SerializeField] private EnemyPool enemyPool; // Tu pool de enemigo 1 o enemigo 2

    [Header("Tiempos de Spawn (Configurables)")]
    [Tooltip("Cada cuántos segundos sale un enemigo.")]
    [SerializeField] private float spawnRate = 5f;

    [Tooltip("Retraso en segundos antes de que salga el primer enemigo al dar Play.")]
    [SerializeField] private float spawnDelay = 2f;

    [Header("Límites del Spawner")]
    [Tooltip("Cantidad máxima de enemigos de este tipo permitidos vivos a la vez en el mapa.")]
    [SerializeField] private int maxEnemiesAlive = 5;

    [Tooltip("Radio aleatorio alrededor de este punto para que no salgan todos encimados.")]
    [SerializeField] private float spawnRadius = 3f;

    private void Start()
    {
        if (enemyPool == null)
        {
            Debug.LogError($"[EnemySpawner] {gameObject.name} no tiene un EnemyPool asignado en el Inspector.");
            return;
        }

        // Iniciamos el temporizador repetitivo usando los segundos que pongas en el Inspector
        InvokeRepeating(nameof(TrySpawnEnemy), spawnDelay, spawnRate);
    }

    private void TrySpawnEnemy()
    {
        if (enemyPool == null) return;

        // Validamos la optimización: Contamos cuántos enemigos de este pool están activos en la escena
        int currentEnemiesAlive = CountActiveEnemiesInPool();

        // Si ya se llegó al límite configurado, cancelamos este intento de spawn y esperamos al siguiente frame de tiempo
        if (currentEnemiesAlive >= maxEnemiesAlive)
        {
            Debug.Log($"[Spawner] Límite alcanzado ({currentEnemiesAlive}/{maxEnemiesAlive}). Esperando a que muera alguno.");
            return;
        }

        // Si hay espacio libre, le pedimos de forma segura el enemigo al pool
        GameObject enemy = enemyPool.GetEnemy();

        if (enemy != null)
        {
            // Calculamos la posición aleatoria en base al radio del Inspector
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                transform.position.x + randomCircle.x,
                transform.position.y,
                transform.position.z + randomCircle.y
            );

            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            // Sincronización obligatoria para la IA con NavMeshAgent
            var agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.Warp(spawnPosition);
        }
    }

    // Función interna que recorre los hijos del Pool para contar cuántos están peleando
    private int CountActiveEnemiesInPool()
    {
        int count = 0;
        // Recorremos todos los objetos que el pool tiene guardados como hijos adentro de su jerarquía
        foreach (Transform child in enemyPool.transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }

    // Dibuja el radio rojo en la ventana Scene para ayudarte al diseño del nivel
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}