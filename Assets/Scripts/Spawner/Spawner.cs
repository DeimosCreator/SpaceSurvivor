using UnityEngine;

namespace Spawner
{
    public class Spawner : MonoBehaviour
    {
        public string basePath = "Sprites/Enemies/Lvl";
        public float interval = 2f;
        public GameObject healthBarPrefab; // UI префаб
        public Transform uiCanvas; // Canvas, в который будем помещать UI

        private GameObject[] enemyPrefabs;
        private float timer;
        private int currentLevel = 1;

        void Start()
        {
            LoadEnemies(currentLevel);
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                SpawnEnemy();
            }
        }

        void SpawnEnemy()
        {
            if (enemyPrefabs.Length == 0) return;

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector2 pos = new Vector2(Random.Range(-8f, 8f), 6f);
            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);

            // Создание UI шкалы
            GameObject bar = Instantiate(healthBarPrefab, uiCanvas);
            bar.GetComponent<Enemies.EnemyHealth>().target = enemy.transform;
        }

        public void SetLevel(int lvl)
        {
            currentLevel = Mathf.Clamp(lvl, 1, 10);
            LoadEnemies(currentLevel);
        }

        void LoadEnemies(int level)
        {
            string path = $"{basePath}{level}";
            enemyPrefabs = Resources.LoadAll<GameObject>(path);
            Debug.Log($"Загружены враги из: {path}, найдено: {enemyPrefabs.Length}");
        }
    }
}