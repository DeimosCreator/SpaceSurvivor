#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TextToTMPScriptConverter : EditorWindow
    {
        [MenuItem("Tools/Convert Text References in Scripts")]
        public static void ConvertTextReferences()
        {
            // Ограничиваем сканирование папками Scripts и Editor
            string[] targetFolders = { "Assets/Scripts", "Assets/Editor" };

            foreach (string folder in targetFolders)
            {
                if (!Directory.Exists(folder)) continue;

                string[] scripts = Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories);

                foreach (string path in scripts)
                {
                    string code = File.ReadAllText(path);
                    string originalCode = code;

                    // Добавляем using TMPro; если его нет
                    if (!code.Contains("using TMPro;"))
                    {
                        code = Regex.Replace(code, @"(using\s+UnityEngine\.UI\s*;)", "using TMPro;\n$1");
                    }

                    // Заменяем только точечную запись TMPro.TextMeshProUGUI
                    code = code.Replace("TMPro.TextMeshProUGUI", "TMPro.TextMeshProUGUI");

                    // Заменяем поля вида: public/private TextMeshProUGUI xxx; -> TextMeshProUGUI xxx;
                    code = Regex.Replace(code, @"\b(Text)\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*;", "TextMeshProUGUI $2;");

                    // Заменяем присваивания и обращения: GetComponent<TextMeshProUGUI>() -> GetComponent<TextMeshProUGUI>()
                    code = Regex.Replace(code, @"GetComponent<\s*Text\s*>", "GetComponent<TextMeshProUGUI>");
                    code = Regex.Replace(code, @"GetComponentInChildren<\s*Text\s*>", "GetComponentInChildren<TextMeshProUGUI>");
                    code = Regex.Replace(code, @"GetComponentInParent<\s*Text\s*>", "GetComponentInParent<TextMeshProUGUI>");

                    if (code != originalCode)
                    {
                        File.WriteAllText(path, code);
                        Debug.Log($"✅ Обновлён скрипт: {path}");
                    }
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
#endif
