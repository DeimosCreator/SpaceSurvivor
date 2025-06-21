#if UNITY_EDITOR
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace Editor
{
    public class TMPFontGenerator : EditorWindow
    {
        [MenuItem("Tools/Generate TMP Fonts")]
        public static void GenerateTMPFonts()
        {
            string[] ttfPaths = Directory.GetFiles("Assets", "*.ttf", SearchOption.AllDirectories);
            string[] otfPaths = Directory.GetFiles("Assets", "*.otf", SearchOption.AllDirectories);
            string[] fontPaths = ttfPaths.Concat(otfPaths).ToArray();

            
            foreach (string fontPath in fontPaths)
            {
                Debug.Log(fontPath);
                Font font = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
                if (font == null) continue;

                string targetPath = Path.ChangeExtension(fontPath, ".asset");

                TMP_FontAsset existingFontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(targetPath);
                if (existingFontAsset != null)
                {
                    Debug.Log($"ℹ️ TMP Font уже существует: {targetPath}");
                    continue;
                }

                TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic);

                if (fontAsset != null)
                {
                    string characters =
                        "abcdefghijklmnopqrstuvwxyz" +
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                        "0123456789" +
                        "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" +
                        "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                        "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~;^:&?!()_-=+\"'";

                    fontAsset.TryAddCharacters(characters);

                    // 🔧 Обязательно сохраняем под-объекты!
                    AssetDatabase.CreateAsset(fontAsset, targetPath);
                    AssetDatabase.AddObjectToAsset(fontAsset.material, targetPath);

                    foreach (var tex in fontAsset.atlasTextures)
                    {
                        if (tex != null)
                            AssetDatabase.AddObjectToAsset(tex, targetPath);
                    }

                    EditorUtility.SetDirty(fontAsset);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Debug.Log($"✅ TMP Font создан: {targetPath}");
                }
                else
                {
                    Debug.LogError($"❌ Не удалось создать TMP Font из: {fontPath}");
                }

            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
}
#endif