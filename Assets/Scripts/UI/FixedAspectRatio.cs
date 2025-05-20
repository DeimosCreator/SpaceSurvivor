using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Camera))]
    public class FixedAspectRatio : MonoBehaviour
    {
        public float targetAspectRatio = 16f / 9f;

        void Start()
        {
            UpdateCameraViewport();
        }

        void Update()
        {
            // Можно вызывать только при изменении размера, но для простоты вызываем каждый кадр
            UpdateCameraViewport();
        }

        void UpdateCameraViewport()
        {
            float screenAspect = (float)Screen.width / Screen.height;

            Camera cam = GetComponent<Camera>();

            if (Mathf.Approximately(screenAspect, targetAspectRatio))
            {
                cam.rect = new Rect(0, 0, 1, 1);
                return;
            }

            if (screenAspect > targetAspectRatio)
            {
                // Экран шире — добавим вертикальные полосы (слева/справа)
                float scaleWidth = targetAspectRatio / screenAspect;
                float offsetX = (1f - scaleWidth) / 2f;
                cam.rect = new Rect(offsetX, 0, scaleWidth, 1);
            }
            else
            {
                // Экран выше — добавим горизонтальные полосы (сверху/снизу)
                float scaleHeight = screenAspect / targetAspectRatio;
                float offsetY = (1f - scaleHeight) / 2f;
                cam.rect = new Rect(0, offsetY, 1, scaleHeight);
            }
        }
    }
}