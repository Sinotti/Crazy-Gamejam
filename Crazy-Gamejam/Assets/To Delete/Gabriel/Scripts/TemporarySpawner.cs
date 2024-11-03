using UnityEngine;

namespace Main.GameSystems
{
    public class TemporarySpawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [Space(6)]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _maxEnemies = 10;
        [SerializeField] private float _spawnRange = 1.0f; 

        private int _currentEnemyCount = 0;

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.F) && _currentEnemyCount < _maxEnemies)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-_spawnRange, _spawnRange),
                0,
                Random.Range(-_spawnRange, _spawnRange)
            );

            Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            _currentEnemyCount++;
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
