using System.Collections.Generic;
using UnityEngine;

namespace Main.GameSystems
{
    public class EnemyPool : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private GameObject enemyPrefab; 
        [SerializeField] private int poolSize = 10; // Every level has your own pool quantity.

        private Queue<GameObject> _enemyPool;

        private void Awake()
        {
            _enemyPool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.SetActive(false);
                _enemyPool.Enqueue(enemy);
            }
        }

        public GameObject GetEnemy()
        {
            if (_enemyPool.Count > 0)
            {
                GameObject enemy = _enemyPool.Dequeue();
                enemy.SetActive(true);
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
        }
    }
}

