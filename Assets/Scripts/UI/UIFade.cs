using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class UIFade : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOut()
        {
            StartCoroutine(FadeToZero(0.5f, null));
        }

        public void FadeOut(Action onComplete)
        {
            StartCoroutine(FadeToZero(0.5f, onComplete));
        }

        public void FadeOut(float duration, Action onComplete = null)
        {
            StartCoroutine(FadeToZero(duration, onComplete));
        }

        private IEnumerator FadeToZero(float duration, Action onComplete)
        {
            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            onComplete?.Invoke();
        }

        public void FadeIn()
        {
            StartCoroutine(FadeToOne(0.5f, null));
        }

        public void FadeIn(Action onComplete)
        {
            StartCoroutine(FadeToOne(0.5f, onComplete));
        }

        public void FadeIn(float duration, Action onComplete = null)
        {
            StartCoroutine(FadeToOne(duration, onComplete));
        }

        private IEnumerator FadeToOne(float duration, Action onComplete)
        {
            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            onComplete?.Invoke();
        }
    }
}