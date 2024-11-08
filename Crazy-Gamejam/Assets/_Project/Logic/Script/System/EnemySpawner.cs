using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Main.GameSystems
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [Header("Spawner Settings")]
        [Space(6)]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private float _spawnTime = 2f;
        [SerializeField] private int _maxEnemiesPerHorde = 10;
        [SerializeField] private int _waves = 5;
        [SerializeField] private int _hordesPerWave = 3;
        [SerializeField] private float _timeBetweenWaves = 5f;

        private float _nextSpawnTime = 0f;
        private int _currentEnemyCount = 0;
        private int _currentWave = 1;
        private int _currentHorde = 1;

        private List<GameObject> _enemies = new List<GameObject>();
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>(); // Lista de pontos de spawn

        private bool _isSpawningWave = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            NextWave();
        }

        public void NextWave()
        {
            if (!_isSpawningWave && _currentWave <= _waves)
            {
                StartCoroutine(SpawnWaves());
            }
        }

        private IEnumerator SpawnWaves()
        {
            _isSpawningWave = true;

            while (_currentWave <= _waves)
            {
                Debug.Log($"Iniciando Wave {_currentWave}");

                for (_currentHorde = 1; _currentHorde <= _hordesPerWave; _currentHorde++)
                {
                    Debug.Log($"Iniciando Horda {_currentHorde} da Wave {_currentWave}");

                    for (int i = 0; i < _maxEnemiesPerHorde; i++)
                    {
                        if (Time.time >= _nextSpawnTime && _currentEnemyCount < _maxEnemiesPerHorde * _hordesPerWave)
                        {
                            SpawnEnemy();
                        }

                        yield return new WaitForSeconds(_spawnTime);
                    }

                    _hordesPerWave++;
                    _enemies.Clear();

                    UIManager.Instance.ToggleStore();
                }

                _currentWave++;
            }

            Debug.Log("Todas as waves foram completadas!");

            _isSpawningWave = false;
        }

        private void SpawnEnemy()
        {
            if (_spawnPoints.Count == 0)
            {
                Debug.LogWarning("Nenhum ponto de spawn foi adicionado.");
                return;
            }

            // Escolhe um ponto de spawn aleatório da lista
            Transform randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            Vector3 spawnPosition = randomSpawnPoint.position;

            GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            if (enemy != null)
            {
                _currentEnemyCount++;
                _enemies.Add(enemy);
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

        public List<GameObject> GetEnemies()
        {
            return _enemies;
        }
    }
}