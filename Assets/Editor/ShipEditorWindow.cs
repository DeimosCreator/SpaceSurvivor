using System.Collections.Generic;
using Player.Parts;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.IO;
using System.Linq;

namespace Editor
{
    public class ShipEditorWindow : EditorWindow
    {
        private ShipTreeView treeView;
        private TreeViewState treeViewState;
        private ShipPart selectedShipPart;
        private ShipPart previousSelectedPart;


        [MenuItem("Window/Ship Editor")]
        public static void ShowWindow()
        {
            GetWindow<ShipEditorWindow>("Ship Editor");
        }

        private void OnEnable()
        {
            treeViewState = new TreeViewState();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Add New Part");

            if (GUILayout.Button("Add Part"))
            {
                AddPartToSelectedShip();
            }

            if (GUILayout.Button("Generate Attach Point Prefabs"))
            {
                if (selectedShipPart != null)
                {
                    CreatePrefabsForEachAttachPoint(selectedShipPart);
                }
            }

            EditorGUILayout.Space();

            // Обновляем выбранный ShipPart
            ShipPart newSelection = (ShipPart)EditorGUILayout.ObjectField("Selected Ship Part", selectedShipPart, typeof(ShipPart), true);

            // Если выбор изменился — пересоздаем дерево
            if (newSelection != selectedShipPart)
            {
                selectedShipPart = newSelection;
                previousSelectedPart = selectedShipPart;

                if (selectedShipPart != null)
                {
                    treeView = new ShipTreeView(treeViewState, selectedShipPart);
                }
                else
                {
                    treeView = null;
                }
            }

            if (selectedShipPart != null)
            {
                if (treeView != null)
                {
                    Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000); // "растягиваемое" пространство под TreeView
                    treeView.OnGUI(rect);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Select a ShipPart component to view the tree.");
            }

            GUILayout.EndVertical();
        }



        private void AddPartToSelectedShip()
        {
            if (selectedShipPart != null)
            {
                // Попытаться прикрепить деталь к точке привязки
                bool partAdded = false;

                foreach (var attachPoint in selectedShipPart.attachPoints)
                {
                    selectedShipPart.TryAttachPart(attachPoint);  // Метод теперь не возвращает bool

                    // Если привязка произошла (по каким-то дополнительным признакам), можно обновить дерево
                    partAdded = true;
                    break;
                }

                if (partAdded)
                {
                    // Обновляем дерево после добавления детали
                    treeView.Reload();
                }
                else
                {
                    Debug.LogWarning("Failed to attach part.");
                }
            }
        }

        private void CreatePrefabsForEachAttachPoint(ShipPart rootPart)
        {
            string basePath = "Assets/GeneratedParts";
            if (!AssetDatabase.IsValidFolder(basePath))
                AssetDatabase.CreateFolder("Assets", "GeneratedParts");

            SavePartRecursive(rootPart.transform, basePath);
        }

 private void SavePartRecursive(Transform currentTransform, string parentPath)
{
    if (currentTransform == null) return;

    ShipPart part = currentTransform.GetComponent<ShipPart>();
    if (part == null) return;

    foreach (Transform attachPoint in part.attachPoints)
    {
        foreach (Transform child in attachPoint)
        {
            if (child == null) continue;

            ShipPart childPart = child.GetComponent<ShipPart>();
            if (childPart != null)
            {
                string folderName = attachPoint.name;
                string targetPath = $"{parentPath}/{folderName}";

                if (!AssetDatabase.IsValidFolder(targetPath))
                    AssetDatabase.CreateFolder(parentPath, folderName);

                // 🛠 СОЗДАЁМ НОВЫЙ ОБЪЕКТ и копируем только первый уровень
                GameObject cleanCopy = new GameObject(child.name);
                ShipPart newPart = cleanCopy.AddComponent<ShipPart>();

                // Копируем attachPoints (пустыми)
                ShipPart originalPart = child.GetComponent<ShipPart>();
                newPart.attachPoints = new List<Transform>();

                for (int i = 0; i < originalPart.attachPoints.Count; i++)
                {
                    Transform originalAttachPoint = originalPart.attachPoints[i];

                    GameObject attachGO = new GameObject(originalAttachPoint.name);
                    attachGO.transform.SetParent(cleanCopy.transform, false);
                    attachGO.transform.localPosition = originalAttachPoint.localPosition;
                    attachGO.transform.localRotation = originalAttachPoint.localRotation;
                    attachGO.transform.localScale = originalAttachPoint.localScale;

                    newPart.attachPoints.Add(attachGO.transform); // <-- исправлено
                }



                // Копируем остальные нужные компоненты (если есть)
                CopyComponentIfExists<MeshFilter>(child.gameObject, cleanCopy);
                CopyComponentIfExists<MeshRenderer>(child.gameObject, cleanCopy);
                CopyComponentIfExists<SpriteRenderer>(child.gameObject, cleanCopy);
                // можно добавить любые нужные MonoBehaviour компоненты

                // Сохраняем как префаб
                string prefabPath = $"{targetPath}/{cleanCopy.name}.prefab";
                PrefabUtility.SaveAsPrefabAsset(cleanCopy, prefabPath);
                Debug.Log($"Saved prefab at: {prefabPath}");

                DestroyImmediate(cleanCopy);

                // Рекурсивно сохраняем вложенные
                SavePartRecursive(child, targetPath);
            }
        }
    }
}

private void CopyComponentIfExists<T>(GameObject source, GameObject destination) where T : Component
{
    T comp = source.GetComponent<T>();
    if (comp != null)
    {
        UnityEditorInternal.ComponentUtility.CopyComponent(comp);
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(destination);
    }
}


    }
}
