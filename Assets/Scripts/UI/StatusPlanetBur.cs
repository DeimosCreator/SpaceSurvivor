using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatusPlanetBur : MonoBehaviour
    {
        public Image image;

        private Color targetColor;
        private Vector3 targetScale;

        private readonly float lerpSpeed = 5f; // Плавность

        private float currentAmount = 100f; // Текущее здоровье в процентах (от 0 до 100)

        private void Awake()
        {
            SetScale(0f); // 0 потерь = 100% здоровье
        }

        private void Update()
        {
            if (image != null)
            {
                // Плавная смена цвета
                image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * lerpSpeed);
                
                // Плавная смена размера именно у Image
                image.rectTransform.localScale = Vector3.Lerp(image.rectTransform.localScale, targetScale, Time.deltaTime * lerpSpeed);
            }
        }

        /// <summary>
        /// Уменьшает здоровье шкалы на заданное количество процентов.
        /// Например, amount = 10 уменьшит здоровье на 10%.
        /// </summary>
        public void SetScale(float amount)
        {
            currentAmount -= amount;
            currentAmount = Mathf.Clamp(currentAmount, 0f, 100f);

            float normalizedAmount = currentAmount / 100f;

            targetColor = Color.Lerp(Color.red, Color.green, normalizedAmount);

            // Масштабируем по Y
            targetScale = new Vector3(1f, normalizedAmount, 1f);
        }
    }
}