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

        void Start()
        {
            cam = Camera.main;
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 worldPos = target.position + offset;
            Vector2 viewportPos = cam.WorldToViewportPoint(worldPos);
            Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

            Vector2 uiPos = new Vector2(
                (viewportPos.x - 0.5f) * canvasSize.x,
                (viewportPos.y - 0.5f) * canvasSize.y
            );

            rectTransform.anchoredPosition = uiPos;
        }


    }
}
