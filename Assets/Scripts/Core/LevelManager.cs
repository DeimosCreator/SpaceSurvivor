using UnityEngine;

namespace Core
{
    public class LevelManager : MonoBehaviour
    {
        public Spawner.Spawner spawner;
        public UI.StatusBar statusBar;
        public float levelDuration = 30f;
        public float spawnSpeedIncrease = 0.2f;
        public float timer;
        
        private int currentLevel = 1;

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= levelDuration)
            {
                timer = 0f;
                currentLevel++;
                currentLevel = Mathf.Min(currentLevel, 10);

                spawner.enemyInterval = Mathf.Max(0.5f, spawner.enemyInterval - spawnSpeedIncrease);
                spawner.SetLevel(currentLevel);
                statusBar.ResetTimer();
                Debug.Log($"Уровень {currentLevel}, Интервал: {spawner.enemyInterval}");
            }
        }

        public int CurrentLevel() => currentLevel;
        public float RemainingTime() => levelDuration - timer;
    }
}