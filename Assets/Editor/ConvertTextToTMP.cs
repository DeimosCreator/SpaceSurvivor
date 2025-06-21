#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public class ConvertTextToTMP : MonoBehaviour
    {
        [MenuItem("Tools/Convert All Text to TMP")]
        public static void ConvertAll()
        {
            Text[] allTexts = GameObject.FindObjectsOfType<Text>(true);
            int count = 0;

            foreach (Text oldText in allTexts)
            {
                GameObject go = oldText.gameObject;
                string textValue = oldText.text;
                Font font = oldText.font;
                Color color = oldText.color;
                int fontSize = oldText.fontSize;
                TextAnchor alignment = oldText.alignment;
                bool raycast = oldText.raycastTarget;

                // Удаляем старый компонент
                DestroyImmediate(oldText, true);

                // Добавляем TMP компонент
                TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
                tmp.text = textValue;
                tmp.color = color;
                tmp.fontSize = fontSize;
                tmp.alignment = ConvertAlignment(alignment);
                tmp.raycastTarget = raycast;

                // Привязываем TMP Font, если есть
                string fontPath = AssetDatabase.GetAssetPath(font);
                string tmpFontPath = System.IO.Path.ChangeExtension(fontPath, ".asset");
                TMP_FontAsset tmpFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(tmpFontPath);
                if (tmpFont != null)
                {
                    tmp.font = tmpFont;
                }

                count++;
            }

            Debug.Log($"🔄 Заменено Text компонентов: {count}");
        }

        private static TextAlignmentOptions ConvertAlignment(TextAnchor anchor)
        {
            return anchor switch
            {
                TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft,
                TextAnchor.UpperCenter => TextAlignmentOptions.Top,
                TextAnchor.UpperRight => TextAlignmentOptions.TopRight,
                TextAnchor.MiddleLeft => TextAlignmentOptions.Left,
                TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
                TextAnchor.MiddleRight => TextAlignmentOptions.Right,
                TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft,
                TextAnchor.LowerCenter => TextAlignmentOptions.Bottom,
                TextAnchor.LowerRight => TextAlignmentOptions.BottomRight,
                _ => TextAlignmentOptions.Center
            };
        }
    }
}
#endif
