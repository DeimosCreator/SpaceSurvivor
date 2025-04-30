using System;
using System.Collections;
using System.Collections.Generic;
using Player.Parts;
using Player.Weapons;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerShooting : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public List<Transform> firePoints = new List<Transform>();
        public GameObject gunFirePoint;

        public int ammo = 33;  // Начальные патроны
        public int maxammo = 100;
        public int gunCount = 1;
        public int wingCount = 0;
        
        private int damage = 5;
        private bool rapidFire = false;
        private GameObject currentWeapon;
        private ShipPartManager shipPartManager;
        private Coroutine shootCoroutine;

        private int maxHealth = 100;
        public static int health = 100; // Начальное здоровье

        // Очередь для деталей, которые нужно добавить
        private Queue<ShipPart> partsToAdd = new Queue<ShipPart>();

        private bool isBeam = false;

        void Awake()
        {
            shipPartManager = gameObject.GetComponent<ShipPartManager>();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && shootCoroutine == null)
            {
                shootCoroutine = StartCoroutine(Short());
            }
            else if (Input.GetKeyUp(KeyCode.Space) && shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
        }


        private IEnumerator Short()
        {
            while (true)
            {
                Shoot();
                if (rapidFire)
                    yield return new WaitForSeconds(0.25f);
                else
                    yield return new WaitForSeconds(0.5f);
            }
        }

        void Shoot()
        {
            foreach (var firePoint in firePoints)
            {
                if (ammo > 0)
                {
                    GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Bullet bullet = bulletObj.GetComponent<Bullet>();
                    bullet.damage = damage;
                    ammo--;
                }
            }
        }

        // Методы для бонусов

        public void AddWing()
        {
            gameObject.GetComponent<PlayerController>().rotationSpeed += 20;
            shipPartManager.AddWing();
        }
        
        public void AddAmmo(int amount)
        {
            ammo += amount * gunCount;
        }

        public void AddDamage(int amount)
        {
            shipPartManager.AddGun(gunFirePoint);
            damage += amount;
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
        public void AddHeal(int amount)
        {
            if (maxHealth - (health + amount) < 0)
            {
                maxHealth += health + amount - maxHealth;
            }
            health = Mathf.Min(health + amount, maxHealth);

            GameObject[] damageEffects = GameObject.FindGameObjectsWithTag("Damage");
            
            int index = Random.Range(0, damageEffects.Length);
            GameObject toRemove = damageEffects[index];
            
            Effects.Effect effect = FindObjectOfType<Effects.Effect>();
            if (effect != null)
            {
                effect.Destroy_Effect(toRemove, 1); 
            }
        }

        // Метод для увеличения скорости
        public void AddSpeed(float amount)
        {
            gameObject.GetComponent<PlayerController>().moveSpeed += amount;
            if (!isBeam)
            {
                shipPartManager.AddBeam();
                isBeam = true;
            }
            else
            {
                shipPartManager.AddEngine();
                isBeam = false;
            }
        }

        // Метод для получения текущего здоровья
        public int GetHealth()
        {
            return health;
        }

        // Метод для получения текущей скорости
        public float GetSpeed()
        {
            return gameObject.GetComponent<PlayerController>().moveSpeed;
        }

        public int GetAmmo()
        {
            return ammo;
        }
    }
}
