using System.Collections.Generic;
using UnityEngine;

namespace Main.GameSystems
{
    public class EnemyPooling : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _poolSize = 10; // Cada nível tem sua própria quantidade de pool.

        // Distância mínima entre os inimigos
        [SerializeField] private float _minSpawnDistance = 2f;

        private Queue<GameObject> _enemyPool;
        private List<Vector3> _spawnedEnemyPositions = new List<Vector3>(); // Armazena as posições dos inimigos ativos

        private void Awake()
        {
            _enemyPool = new Queue<GameObject>();

            for (int i = 0; i < _poolSize; i++)
            {
                GameObject enemy = Instantiate(_enemyPrefab);
                enemy.SetActive(false);
                _enemyPool.Enqueue(enemy);
            }
        }

        public GameObject GetEnemy(Vector3 spawnPosition)
        {
            if (_enemyPool.Count > 0)
            {
                GameObject enemy = _enemyPool.Dequeue();
                enemy.SetActive(true);

                // Gera uma posição válida para o inimigo
                Vector3 validPosition = GetValidSpawnPosition(spawnPosition);
                enemy.transform.position = validPosition;
                _spawnedEnemyPositions.Add(validPosition); // Adiciona à lista de inimigos ativos

                return enemy;
            }
            else
            {
                Debug.LogWarning("Empty Pool");
                return null;
            }
        }

        public void ReturnEnemy(GameObject enemy)
        {
            enemy.SetActive(false);
            _enemyPool.Enqueue(enemy);

            // Remove a posição do inimigo retornado
            if (_spawnedEnemyPositions.Contains(enemy.transform.position))
            {
                _spawnedEnemyPositions.Remove(enemy.transform.position);
            }
        }

        private Vector3 GetValidSpawnPosition(Vector3 spawnPosition)
        {
            // Tenta encontrar uma posição válida em torno da posição de spawn
            Vector3 newPosition;
            int attempts = 0;

            do
            {
                // Gera uma posição aleatória dentro de um certo raio
                float randomX = Random.Range(-_minSpawnDistance, _minSpawnDistance);
                float randomZ = Random.Range(-_minSpawnDistance, _minSpawnDistance);
                newPosition = spawnPosition + new Vector3(randomX, 0, randomZ);

                attempts++;
            } while (IsPositionOccupied(newPosition) && attempts < 10); // Tenta até 10 vezes

            return newPosition; // Retorna a posição válida ou a posição original se não conseguir encontrar
        }

        private bool IsPositionOccupied(Vector3 position)
        {
            // Verifica se a nova posição está ocupada por outro inimigo
            foreach (Vector3 enemyPosition in _spawnedEnemyPositions)
            {
                if (Vector3.Distance(position, enemyPosition) < _minSpawnDistance)
                {
                    return true; // Posição ocupada
                }
            }
            return false; // Posição livre
        }
    }
}
