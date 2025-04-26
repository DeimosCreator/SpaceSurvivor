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
        
        private List<Transform> wingRoots = new List<Transform>(); // корни для поиска attach-точек

        private void Start()
        {
            wingRoots.Add(transform); // изначально корень — сам корабль
        }

        public void AddWing()
        {
            PlayerShooting playerShooting = gameObject.GetComponent<PlayerShooting>();
            if (playerShooting.wingCount >= 20) return;
            playerShooting.wingCount++;
            AttachNextFromList(availableWingPaths, ref currentWingIndex, wingRoots);
        }

        public void AddEngine()
        {
            AttachNextFromList(availableEnginePaths, ref currentEngineIndex, wingRoots);
        }
        
        public void AddBeam()
        {
            AttachNextFromList(availableBeamPaths, ref currentBeamIndex, wingRoots);
        }

        public void AddGun(GameObject firePoint)
        {
            AttachNextFromList(availableGunPaths, ref currentGunIndex, wingRoots, firePoint);
        }
        
        private void AttachNextFromList(List<string> partPaths, ref int index, List<Transform> possibleParents, GameObject child = null)
        {
            if (partPaths.Count == 0) return;

            string path = partPaths[index];
            index = (index + 1) % partPaths.Count; // по кругу

            AttachPart(path, possibleParents, child);
        }

        private void AttachPart(string fullPath, List<Transform> possibleParents, GameObject child = null)
        {
            GameObject partPrefab = Resources.Load<GameObject>(fullPath);
            if (partPrefab == null)
            {
                Debug.LogWarning($"Part prefab not found at path '{fullPath}'");
                return;
            }

            // Направление из пути: Sprites/Player/Parts/left/wingBlue_0 → "left"
            string[] pathParts = fullPath.Split('/');
            if (pathParts.Length < 5)
            {
                Debug.LogWarning("Invalid path format: " + fullPath);
                return;
            }

            string direction = pathParts[^2];


            Transform attachPoint = null;

            // Ищем подходящую attach-точку среди всех родителей
            foreach (Transform root in possibleParents)
            {
                attachPoint = FindAttachPointRecursive(root, direction);
                if (attachPoint != null)
                    break;
            }

            if (attachPoint == null)
            {
                Debug.LogWarning($"No available attach point for direction '{direction}'");
                return;
            }

            GameObject part = Instantiate(partPrefab, attachPoint);
            part.name = partPrefab.name;
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.identity;
            if (child)
            {
                GameObject partChild = Instantiate(child, part.transform);
                partChild.name = child.name;
                partChild.transform.localPosition = Vector3.zero;
                partChild.transform.localRotation = Quaternion.identity;
                PlayerShooting playerShooting = gameObject.GetComponent<PlayerShooting>();
                playerShooting.firePoints.Add(partChild.transform);
                playerShooting.gunCount++;
                playerShooting.maxammo = 100 * playerShooting.gunCount;
                playerShooting.ammo += 33;
            }

            // Добавляем как новый корень для будущих деталей
            possibleParents.Add(part.transform);

            Debug.Log($"Attached '{part.name}' to direction '{direction}' from path '{fullPath}'");
        }

        private Transform FindAttachPointRecursive(Transform parent, string direction)
        {
            foreach (Transform child in parent)
            {
                if (child.name.ToLower() == direction.ToLower() && child.childCount == 0)
                    return child;

                Transform result = FindAttachPointRecursive(child, direction);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
