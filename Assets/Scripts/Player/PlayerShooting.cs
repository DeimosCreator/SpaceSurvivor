using System;
using System.Collections.Generic;
using Player.Parts;
using Player.Weapons;
using UnityEngine;

namespace Player
{
    public class PlayerShooting : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform firePoint;

        private int ammo = 33;  // Начальные патроны
        private bool doubleDamage = false;
        private bool rapidFire = false;
        private float speed = 5f; // Начальная скорость игрока
        private GameObject currentWeapon;
        private ShipPartManager shipPartManager;
        
        public static int health = 100; // Начальное здоровье

        // Очередь для деталей, которые нужно добавить
        private Queue<ShipPart> partsToAdd = new Queue<ShipPart>();

        void Awake()
        {
            shipPartManager = gameObject.GetComponent<ShipPartManager>();
        }

        // Методы для стрельбы
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && ammo > 0)
            {
                Shoot();
                shipPartManager.AddEngine();
            }

            // Обработка добавленных деталей
            HandleQueuedParts();
        }

        void Shoot()
        {
            // Отправляем пулю в зависимости от текущего состояния
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Если бонус "DoubleDamage" активен — наносим больше урона
            Bullet bullet = bulletPrefab.GetComponent<Bullet>();
            if (bullet != null && doubleDamage)
            {
                bullet.damage *= 2;
            }

            ammo--;
        }

        // Методы для бонусов
        public void AddAmmo(int amount)
        {
            ammo += amount;
        }

        public void DoubleDamage(float multiplier)
        {
            doubleDamage = true;
            Invoke("DisableDoubleDamage", multiplier);
        }

        void DisableDoubleDamage()
        {
            doubleDamage = false;
        }

        public void EnableRapidFire()
        {
            rapidFire = true;
            // Включаем быструю стрельбу
            Invoke("DisableRapidFire", 5f);
        }

        void DisableRapidFire()
        {
            rapidFire = false;
        }

        // Метод для добавления здоровья
        public void Heal(int amount)
        {
            shipPartManager.AddWing();
            health = Mathf.Min(health + amount, 100); // Максимум 100 здоровья
        }

        // Метод для увеличения скорости
        public void IncreaseSpeed(float amount)
        {
            speed += amount;
            shipPartManager.AddEngine();
        }

        // Метод для получения нового оружия
        public void EquipNewWeapon(GameObject newWeapon)
        {
            if (currentWeapon != null)
            {
                Destroy(currentWeapon); // Удалить текущее оружие
            }

            currentWeapon = Instantiate(newWeapon, firePoint.position, firePoint.rotation); // Даем новое оружие
        }

        // Метод для получения текущего здоровья
        public int GetHealth()
        {
            return health;
        }

        // Метод для получения текущей скорости
        public float GetSpeed()
        {
            return speed;
        }

        public int GetAmmo()
        {
            return ammo;
        }

        // Метод для добавления детали
        public void AddPartToShip(ShipPart newPart, Transform attachPoint)
        {
            // Проверяем, есть ли точка крепления
            if (attachPoint == null)
            {
                // Если точки нет — ставим в очередь
                partsToAdd.Enqueue(newPart);
            }
            else
            {
                // Если точка есть — добавляем деталь немедленно
                Instantiate(newPart.gameObject, attachPoint.position, attachPoint.rotation);
            }
        }

        // Метод для обработки очереди добавления деталей
        private void HandleQueuedParts()
        {
            // Просматриваем все детали в очереди
            for (int i = 0; i < partsToAdd.Count; i++)
            {
                ShipPart partToAdd = partsToAdd.Dequeue();
                Transform attachPoint = FindAvailableAttachPoint();

                // Если точка была найдена, добавляем деталь
                if (attachPoint != null)
                {
                    Instantiate(partToAdd.gameObject, attachPoint.position, attachPoint.rotation);
                }
                else
                {
                    // Если точка не найдена, ставим обратно в очередь для повторной попытки
                    partsToAdd.Enqueue(partToAdd);
                }
            }
        }

        // Метод для поиска доступной точки крепления
        private Transform FindAvailableAttachPoint()
        {
            // Проходим по всем точкам крепления на корабле
            foreach (Transform attachPoint in transform) // transform — это сам объект корабля
            {
                // Проверяем, свободна ли точка крепления (например, нет ли уже дочернего объекта на этой точке)
                if (attachPoint.childCount == 0) // Если в точке крепления нет дочерних объектов
                {
                    // Эта точка свободна, возвращаем её
                    return attachPoint;
                }
            }

            // Если все точки крепления заняты, возвращаем null
            return null;
        }

    }
}
