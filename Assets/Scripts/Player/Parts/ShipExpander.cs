using System.Collections.Generic;
using UnityEngine;

namespace Player.Parts
{
    public class ShipExpander : MonoBehaviour
    {
        public List<GameObject> availableParts; // Префабы крыльев, пушек и т.д.
        private List<ShipPart> allParts = new List<ShipPart>();
        
        void Start()
        {
            // Найдём корень (начало корабля)
            ShipPart root = GetComponentInChildren<ShipPart>();
            if (root != null)
            {
                allParts.Add(root);
            }
        }

        public void AddRandomPart()
        {
            if (allParts.Count == 0) return;

            GameObject partPrefab = availableParts[Random.Range(0, availableParts.Count)];

            foreach (var part in allParts)
            {
                GameObject newPartGo = Instantiate(partPrefab); // Инстанцируем префаб
                bool attached = part.TryAttachPart(newPartGo.transform); // Пытаемся прикрепить

                if (attached)
                {
                    ShipPart newPart = newPartGo.GetComponent<ShipPart>();
                    if (newPart != null)
                        allParts.Add(newPart); // Добавляем в список для дальнейшего расширения

                    break;
                }
                else
                {
                    Destroy(newPartGo); // Если не удалось прикрепить — уничтожаем
                }
            }
        }

    }
}