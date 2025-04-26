using System;
using Player.Weapons;
using UnityEngine;


namespace Debuging
{
    public class LaserDebug : MonoBehaviour
    {
        public Bullet bullet;

        private int damage;

        private void Awake()
        {
            damage = bullet.damage;
        }

        void Update()
        {
            if (damage != bullet.damage)
            {
                Debug.LogError("Урон изменился!!!");
            }
        }
    }
}
