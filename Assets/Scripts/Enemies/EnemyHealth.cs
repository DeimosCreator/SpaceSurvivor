using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        private RectTransform rectTransform;
        private Camera cam;
        private Canvas canvas;

        private CanvasGroup group;

        void Start()
        {
            cam = Camera.main;
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            group = gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0f; // скрыта в начале
        }

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            if (group.alpha < 1f && target.position != Vector3.zero)
                group.alpha = 1f; // показать, когда target уже имеет координаты

            Vector3 worldPos = target.position + offset;
            Vector2 screenPos = cam.WorldToScreenPoint(worldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                cam,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint;
        }
    }
}
