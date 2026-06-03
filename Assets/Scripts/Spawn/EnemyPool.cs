using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [Header("Configuración del Pool")]
    [SerializeField] private GameObject enemyPrefab; // Prefab de tu enemigo con IA
    [SerializeField] private int poolSize = 10;        // Cantidad inicial reservada

    private List<GameObject> _pool = new List<GameObject>();

    void Start()
    {
        // Llenamos el pool con enemigos desactivados al iniciar el nivel
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false); // Oculto por defecto
            _pool.Add(enemy);
        }
    }

    // Función para solicitar un enemigo disponible
    public GameObject GetEnemy()
    {
        foreach (var enemy in _pool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        // Si el pool se queda corto, creamos uno extra por seguridad (KISS)
        GameObject newEnemy = Instantiate(enemyPrefab, transform);
        _pool.Add(newEnemy);
        return newEnemy;
    }
}