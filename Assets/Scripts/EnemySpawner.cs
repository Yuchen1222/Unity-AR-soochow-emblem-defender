using UnityEngine;

namespace YUCHEN
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;              // 拖入石頭人 prefab
        public Transform[] spawnPoints;             // 拖入三個生成點
        public float spawnInterval = 3f;            // 產怪間隔（秒）

        void Start()
        {
            InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
        }

        void SpawnEnemy()
        {
            if (enemyPrefab == null || spawnPoints.Length == 0)
                return;

            int index = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);
        }
    }
}
