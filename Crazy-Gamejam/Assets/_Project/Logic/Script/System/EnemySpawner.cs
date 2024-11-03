using UnityEngine;

namespace Main.GameSystems
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [Space(6)]
        [SerializeField] private EnemyPooling _enemyPool;
        [SerializeField] private float _spawnTime = 2f;
        [SerializeField] private int _maxEnemies = 10;

        private float _nextSpawnTime = 0f;
        private int _currentEnemyCount = 0;

        private void Update()
        {
            if (Time.time >= _nextSpawnTime && _currentEnemyCount < _maxEnemies)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Vector3 spawnPosition = transform.position;
            GameObject enemy = _enemyPool.GetEnemy(spawnPosition);

            if (enemy != null)
            {
                _currentEnemyCount++;
                _nextSpawnTime = Time.time + _spawnTime;
            }
        }

        public void DecrementEnemyCount()
        {
            if (_currentEnemyCount > 0)
            {
                _currentEnemyCount--;
            }
        }
    }
}
