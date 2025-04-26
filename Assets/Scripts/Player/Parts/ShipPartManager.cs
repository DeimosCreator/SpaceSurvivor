using System.Collections.Generic;
using UnityEngine;

namespace Player.Parts
{
    public class ShipPartManager : MonoBehaviour
    {
        public List<string> availableWingPaths = new List<string>();
        public List<string> availableEnginePaths = new List<string>();
        public List<string> availableBeamPaths = new List<string>();
        public List<string> availableGunPaths = new List<string>();

        private int currentWingIndex = 0;
        private int currentEngineIndex = 0;
        private int currentBeamIndex = 0;
        private int currentGunIndex = 0;

        private List<Transform> attachRoots = new List<Transform>();
        private Queue<(string path, GameObject child)> pendingParts = new Queue<(string, GameObject)>();

        private void Start()
        {
            attachRoots.Add(transform);
        }

        public void AddWing()
        {
            var playerShooting = GetComponent<PlayerShooting>();
            if (playerShooting.wingCount >= 20) return;

            playerShooting.wingCount++;
            AttachNextFromList(availableWingPaths, ref currentWingIndex);
        }

        public void AddEngine()
        {
            AttachNextFromList(availableEnginePaths, ref currentEngineIndex);
        }

        public void AddBeam()
        {
            AttachNextFromList(availableBeamPaths, ref currentBeamIndex);
        }

        public void AddGun(GameObject firePoint)
        {
            AttachNextFromList(availableGunPaths, ref currentGunIndex, firePoint);
        }

        private void AttachNextFromList(List<string> partPaths, ref int index, GameObject child = null)
        {
            if (partPaths.Count == 0) return;

            string path = partPaths[index];
            index = (index + 1) % partPaths.Count;

            TryAttachPart(path, child);
        }

        private void TryAttachPart(string fullPath, GameObject child = null)
        {
            if (!AttachPart(fullPath, child))
            {
                pendingParts.Enqueue((fullPath, child));
            }
        }

        private bool AttachPart(string fullPath, GameObject child = null)
        {
            GameObject partPrefab = Resources.Load<GameObject>(fullPath);
            if (partPrefab == null)
            {
                Debug.LogWarning($"Part prefab not found at path '{fullPath}'");
                return true;
            }

            string[] pathParts = fullPath.Split('/');
            if (pathParts.Length < 5)
            {
                Debug.LogWarning($"Invalid path format: {fullPath}");
                return true;
            }

            string category = pathParts[^3]; // <- категория (например "beam" или "engine")
            string direction = pathParts[^2]; // <- направление (например "bottom", "left")

            Transform attachPoint = null;

            foreach (var root in attachRoots)
            {
                attachPoint = FindAttachPointRecursive(root, direction, category);
                if (attachPoint != null)
                    break;
            }

            if (attachPoint == null)
            {
                Debug.LogWarning($"No available attach point for direction '{direction}' and category '{category}'");
                return false;
            }

            GameObject part = Instantiate(partPrefab, attachPoint);
            part.name = partPrefab.name;
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.identity;

            if (child != null)
            {
                GameObject partChild = Instantiate(child, part.transform);
                partChild.name = child.name;
                partChild.transform.localPosition = Vector3.zero;
                partChild.transform.localRotation = Quaternion.identity;

                var playerShooting = GetComponent<PlayerShooting>();
                playerShooting.firePoints.Add(partChild.transform);
                playerShooting.gunCount++;
                playerShooting.maxammo = 100 * playerShooting.gunCount;
                playerShooting.ammo += 33;
            }

            attachRoots.Add(part.transform);
            Debug.Log($"[ShipPartManager] Attached '{part.name}' at '{direction}' for category '{category}' from '{fullPath}'");

            AttachPendingParts();
            return true;
        }

        private void AttachPendingParts()
        {
            if (pendingParts.Count == 0)
                return;

            Queue<(string path, GameObject child)> remaining = new Queue<(string, GameObject)>();

            while (pendingParts.Count > 0)
            {
                var (path, child) = pendingParts.Dequeue();
                if (!AttachPart(path, child))
                    remaining.Enqueue((path, child));
            }

            pendingParts = remaining;
        }

        private Transform FindAttachPointRecursive(Transform parent, string direction, string requiredCategory)
        {
            foreach (Transform child in parent)
            {
                if (child.name.ToLower() == direction.ToLower())
                {
                    // Если точка ПУСТА или в ней стоит правильная категория
                    if (child.childCount == 0)
                        return child;

                    // Либо проверяем, что уже прикреплён правильный тип (например beam к beam)
                    var existingPart = child.GetChild(0);
                    if (existingPart.name.ToLower().Contains(requiredCategory.ToLower()))
                        continue; // уже занято нужной категорией
                }

                Transform found = FindAttachPointRecursive(child, direction, requiredCategory);
                if (found != null)
                    return found;
            }

            return null;
        }
    }
}
