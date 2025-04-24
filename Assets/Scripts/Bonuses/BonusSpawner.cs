using UnityEngine;

namespace Bonuses
{
    public class BonusSpawner : MonoBehaviour
    {
        public GameObject[] bonusPrefabs;  // Массив префабов бонусов
        public float spawnInterval = 5f;   // Интервал между спавнами

        private float timer;

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0;
                SpawnBonus();
            }
        }

        void SpawnBonus()
        {
            GameObject bonus = bonusPrefabs[Random.Range(0, bonusPrefabs.Length)];
            Vector2 spawnPosition = new Vector2(Random.Range(-8f, 8f), 6f);
            Instantiate(bonus, spawnPosition, Quaternion.identity);
        }
    }
}