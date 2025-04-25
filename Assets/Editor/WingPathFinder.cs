using System.IO;
using Player.Parts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ShipPartManager))]
    public class ShipPartManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ShipPartManager manager = (ShipPartManager)target;

            if (GUILayout.Button("Найти пути к wingBlue_0"))
            {
                // Папка, в которой мы ищем файлы
                string rootFolder = Application.dataPath + "/Resources/Sprites/Player/Parts";
                // Получаем все файлы с именем wingBlue_0.prefab в папке и всех её подкаталогах
                string[] allFiles = Directory.GetFiles(rootFolder, "wingBlue_0.prefab", SearchOption.AllDirectories);

                manager.availableWingPaths.Clear();

                foreach (string fullPath in allFiles)
                {
                    // Преобразуем путь в относительный
                    string relativePath = fullPath.Replace(Application.dataPath + "/Resources/", "");
                    // Убираем расширение .prefab
                    relativePath = Path.ChangeExtension(relativePath, null);
                    // Заменяем все обратные слэши на прямые
                    relativePath = relativePath.Replace("\\", "/");

                    // Добавляем путь в список доступных путей
                    manager.availableWingPaths.Add(relativePath);
                    Debug.Log("Найден путь: " + relativePath);
                }

                EditorUtility.SetDirty(manager); // Пометить объект как изменённый
            }

            // Если есть доступные пути, показываем их в инспекторе
            if (manager.availableWingPaths.Count > 0)
            {
                GUILayout.Space(10);
                GUILayout.Label("Найденные пути:", EditorStyles.boldLabel);
                foreach (var path in manager.availableWingPaths)
                {
                    GUILayout.Label("• " + path);
                }
            }
            
            if (GUILayout.Button("Найти пути к beam0"))
            {
                string rootFolder = Application.dataPath + "/Resources/Sprites/Player/Parts";
                string[] allFiles = Directory.GetFiles(rootFolder, "beam0.prefab", SearchOption.AllDirectories);

                manager.availableBeamPaths.Clear();

                foreach (string fullPath in allFiles)
                {
                    string relativePath = fullPath.Replace(Application.dataPath + "/Resources/", "");
                    relativePath = Path.ChangeExtension(relativePath, null);
                    relativePath = relativePath.Replace("\\", "/");

                    manager.availableBeamPaths.Add(relativePath);
                    Debug.Log("Найден путь: " + relativePath);
                }

                EditorUtility.SetDirty(manager);
            }
            
            
            if (GUILayout.Button("Найти пути к engine1"))
            {
                string rootFolder = Application.dataPath + "/Resources/Sprites/Player/Parts";
                string[] allFiles = Directory.GetFiles(rootFolder, "engine1.prefab", SearchOption.AllDirectories);

                manager.availableEnginePaths.Clear();

                foreach (string fullPath in allFiles)
                {
                    string relativePath = fullPath.Replace(Application.dataPath + "/Resources/", "");
                    relativePath = Path.ChangeExtension(relativePath, null);
                    relativePath = relativePath.Replace("\\", "/");

                    manager.availableEnginePaths.Add(relativePath);
                    Debug.Log("Найден путь: " + relativePath);
                }

                EditorUtility.SetDirty(manager);
            }

        }
    }
}
