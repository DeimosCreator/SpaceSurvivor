using UnityEngine;
using System.Collections.Generic;

namespace Spawner
{
    public class Spawner : MonoBehaviour
    {
        public string basePath = "Sprites/Enemies/Lvl";  // Путь к врагам
        public string meteorPath = "Sprites/Obstacles";    // Путь к метеорам
        public float enemyInterval = 2f;
        public float meteorInterval = 3f; // Интервал спавна метеоров

        public GameObject healthBarPrefab; // UI префаб для врагов
        public Transform uiCanvas; // Canvas для UI врагов
        private GameObject[] enemyPrefabs;
        private GameObject[] meteorPrefabs;  // Префабы метеоров

        private List<Transform> activeEnemies = new List<Transform>();

        private float enemyTimer;
        private float meteorTimer;
        private int currentLevel = 1;

        void Start()
        {
            LoadEnemies(currentLevel);
            LoadMeteors();  // Загрузка метеоров
        }

        void Update()
        {
            enemyTimer += Time.deltaTime;
            meteorTimer += Time.deltaTime;

            if (enemyTimer >= enemyInterval)
            {
                enemyTimer = 0;
                SpawnEnemy();
            }

            if (meteorTimer >= meteorInterval)
            {
                meteorTimer = 0;
                SpawnMeteor();
            }

            CleanEnemyList();
        }

        void SpawnEnemy()
        {
            GameObject enemy;

            if (currentLevel <= 10)
            {
                if (enemyPrefabs.Length == 0) return;

                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                enemy = Instantiate(prefab, new Vector2(Random.Range(-8f, 8f), 6f), Quaternion.identity);
            }
            else
            {
                // Создание случайного врага с параметрами по уровню
                enemy = CreateRandomEnemy(currentLevel);
            }

            // Создание UI шкалы здоровья
            GameObject bar = Instantiate(healthBarPrefab, uiCanvas);
            bar.GetComponent<Enemies.EnemyHealth>().target = enemy.transform;

            activeEnemies.Add(enemy.transform);
        }


        void SpawnMeteor()
        {
            if (meteorPrefabs.Length == 0) return;

            Vector2 spawnPos;
            int attempts = 10;

            do
            {
                // Случайная позиция
                float x = Random.Range(-8f, 8f);
                spawnPos = new Vector2(x, 6f);

                bool collides = false;
                foreach (var enemy in activeEnemies)
                {
                    if (enemy == null) continue;
                    if (Mathf.Abs(enemy.position.x - x) < 2f)
                    {
                        collides = true;
                        break;
                    }
                }

                if (!collides) break;
                attempts--;

            } while (attempts > 0);

            // Случайно выбираем метеор из списка
            GameObject selectedMeteor = meteorPrefabs[Random.Range(0, meteorPrefabs.Length)];
            Instantiate(selectedMeteor, spawnPos, Quaternion.identity);
        }

        void LoadEnemies(int level)
        {
            string path = $"{basePath}{level}";
            enemyPrefabs = Resources.LoadAll<GameObject>(path);
            Debug.Log($"Загружены враги из: {path}, найдено: {enemyPrefabs.Length}");
        }

        void LoadMeteors()
        {
            meteorPrefabs = Resources.LoadAll<GameObject>(meteorPath);
            Debug.Log($"Загружены метеоры из: {meteorPath}, найдено: {meteorPrefabs.Length}");
        }

        void CleanEnemyList()
        {
            // Очищаем список от уничтоженных врагов
            activeEnemies.RemoveAll(e => e == null);
        }

        public void SetLevel(int lvl)
        {
            currentLevel = Mathf.Clamp(lvl, 1, 10);
            LoadEnemies(currentLevel);
        }
        
        GameObject CreateRandomEnemy(int level)
        {
            GameObject baseEnemy = Resources.Load<GameObject>("Sprites/Enemies/BaseEnemy");

            if (baseEnemy == null)
            {
                Debug.LogError("Не найден BaseEnemy в Resources/Sprites/Enemies/");
                return null;
            }

            GameObject newEnemy = Instantiate(baseEnemy, new Vector2(Random.Range(-8f, 8f), 6f), Quaternion.identity);

            // Масштабный прирост HP и урона
            int hp = 100 + (level - 10) * 20;

            var enemyStats = newEnemy.GetComponent<Enemies.Enemy>();
            if (enemyStats != null)
            {
                enemyStats.SetStats(hp);
            }

            return newEnemy;
        }

    }
}
