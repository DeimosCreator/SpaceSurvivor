using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System.Collections.Generic;
using Player.Parts;

namespace Editor
{
    public class ShipTreeView : TreeView
    {
        public ShipPart shipPart;

        public ShipTreeView(TreeViewState state, ShipPart shipPart) : base(state)
        {
            this.shipPart = shipPart;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var rootItem = new TreeViewItem { id = 0, depth = -1, displayName = "Ship Parts" };

            // Инициализируем список дочерних элементов
            rootItem.children = new List<TreeViewItem>();

            // Добавляем все точки привязки как дочерние элементы
            AddAttachPoints(rootItem);

            return rootItem;
        }

        private void AddAttachPoints(TreeViewItem parentItem)
        {
            for (int i = 0; i < shipPart.attachPoints.Count; i++)
            {
                var attachPoint = shipPart.attachPoints[i];
                var newItem = new TreeViewItem { id = i + 1, depth = 0, displayName = attachPoint.gameObject.name }; // Используем имя объекта
                parentItem.AddChild(newItem); // Добавляем дочерний элемент для точки привязки

                // Добавляем дочерние элементы для каждой детали, если они есть
                AddChildParts(newItem, attachPoint, 1); // Передаем уровень отступа (depth)
            }
        }

        private void AddChildParts(TreeViewItem parentItem, Transform attachPoint, int currentDepth)
        {
            int childId = parentItem.id + 1000; // Создаем уникальные id для дочерних элементов
            foreach (Transform child in attachPoint)
            {
                var newChildItem = new TreeViewItem { id = childId++, depth = currentDepth, displayName = child.gameObject.name }; // Используем имя объекта
                parentItem.AddChild(newChildItem); // Добавляем дочерний элемент детали

                // Рекурсивно добавляем дочерние объекты для дочерних объектов
                AddChildParts(newChildItem, child, currentDepth + 1); // Увеличиваем уровень вложенности
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item;
            base.RowGUI(args);
        }
    }
}
