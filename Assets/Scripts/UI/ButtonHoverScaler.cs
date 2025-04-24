using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonHoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 originalScale;
        public float scaleFactor = 1.5f;
        public float speed = 10f;

        private Vector3 targetScale;

        void Start()
        {
            originalScale = transform.localScale;
            targetScale = originalScale;
        }

        void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            targetScale = originalScale * scaleFactor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            targetScale = originalScale;
        }
    }
}