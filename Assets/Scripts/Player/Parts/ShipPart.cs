using System.Collections.Generic;
using UnityEngine;

namespace Player.Parts
{
    public class ShipPart : MonoBehaviour
    {
        // Список точек привязки для этой части
        public List<Transform> attachPoints = new List<Transform>();

        // Метод для прикрепления детали
        public bool TryAttachPart(Transform part)
        {
            if (attachPoints.Count == 0)
            {
                Debug.LogWarning("No available attach points.");
                return false; // Изменено на false, чтобы явно показать, что ничего не прикреплено
            }

            foreach (var attachPoint in attachPoints)
            {
                Debug.Log("Checking attach point: " + attachPoint.name);
                if (CanAttach(attachPoint))
                {
                    Debug.Log("Can attach to: " + attachPoint.name);
                    Attach(part, attachPoint);
                    return true;
                }
            }


            Debug.LogWarning("No suitable attach point found.");
            return false; // Если не нашли подходящей точки привязки
        }



        public bool CanAttach(Transform attachPoint)
        {
            return attachPoint != null && attachPoint.childCount == 0;
        }


        private void Attach(Transform part, Transform attachPoint)  // Используем Transform вместо GameObject
        {
            part.SetParent(attachPoint);  // Используем SetParent для привязки
            part.localPosition = Vector3.zero;  // Расположим на точке привязки
            part.localRotation = Quaternion.identity;  // Сбросим ориентацию
            Debug.Log("Part attached to " + attachPoint.name);
        }

    }
}