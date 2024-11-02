using UnityEngine;

namespace Main.GameSystems
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [Space(6)]
        [SerializeField] private EnemyPool enemyPool; 
        [SerializeField] private float spawnTime = 2f;
        [SerializeField] private int maxEnemies = 10;

        private float _nextSpawnTime = 0f; 
        private int _currentEnemyCount = 0;

        private GameObject _enemy;

        private void Update()
        {
            if (Time.time >= _nextSpawnTime && _currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            _enemy = enemyPool.GetEnemy();

            if (_enemy != null)
            {
                _enemy.transform.position = transform.position;
                _currentEnemyCount++; 

                _nextSpawnTime = Time.time + spawnTime;
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
