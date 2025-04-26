using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatusBar : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image healthBar;
        public Image ammoBar;
        public Image timeBar;

        [Header("Player Reference")]
        public Player.PlayerShooting playerShooting;
        public Core.LevelManager levelManager;

        [Header("Timer Settings")]
        public float maxTime = 30f; // Максимальная длительность уровня
        public bool autoDecreaseTime = true;

        private float currentTime;

        void Start()
        {
            currentTime = maxTime;

            // Установка начальных fillOrigin (0 — слева, 1 — справа)
            if (healthBar != null) healthBar.fillOrigin = 0;
            if (ammoBar != null) ammoBar.fillOrigin = 0;
            if (timeBar != null) timeBar.fillOrigin = 1;
        }

        void Update()
        {
            if (playerShooting == null) return;

            UpdateHealthBar();
            UpdateAmmoBar();

            if (autoDecreaseTime)
                UpdateTimeBar();
        }

        void UpdateHealthBar()
        {
            if (healthBar == null) return;

            float healthPercent = Mathf.Clamp01(playerShooting.GetHealth() / 100f);
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthPercent, Time.deltaTime * 10f);
        }

        void UpdateAmmoBar()
        {
            if (ammoBar == null) return;

            float ammoPercent = Mathf.Clamp01(playerShooting.GetAmmo() / (float)playerShooting.maxammo);
            ammoBar.fillAmount = Mathf.Lerp(ammoBar.fillAmount, ammoPercent, Time.deltaTime * 10f);
        }

        void UpdateTimeBar()
        {
            if (timeBar == null) return;

            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(0f, currentTime);

            float timePercent = Mathf.Clamp01(currentTime / maxTime);
            timeBar.fillAmount = Mathf.Lerp(timeBar.fillAmount, timePercent, Time.deltaTime * 10f);
        }

        // Внешний вызов сброса таймера (если нужно начать уровень заново)
        public void ResetTimer()
        {
            currentTime = maxTime;
        }

        public float GetCurrentTime() => currentTime;
    }
}
