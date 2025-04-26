using Player;
using Player.Parts;
using UnityEngine;

public enum BonusType { ExtraAmmo, DoubleDamage, RapidFire, Health, Speed, NewWeapon }

namespace Bonuses
{
    public class Bonus : MonoBehaviour
    {
        public BonusType type;
        public float speed = 2f;  // Скорость падения бонуса

        void Update()
        {
            // Бонус двигается вниз
            transform.Translate(Vector2.down * (speed * Time.deltaTime)); 

            // Уничтожение бонуса, если он выходит за экран
            if (transform.position.y < -6f)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ApplyBonus(other.gameObject); // Применить бонус
                Destroy(gameObject); // Уничтожить бонус после применения
            }
        }

        void ApplyBonus(GameObject player)
        {
            PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();

            if (playerShooting == null) return;

            switch (type)
            {
                case BonusType.ExtraAmmo:
                    playerShooting.AddAmmo(20); // Добавить патроны
                    break;
                case BonusType.DoubleDamage:
                    playerShooting.AddDamage(5); // Увеличить урон
                    break;
                case BonusType.RapidFire:
                    playerShooting.EnableRapidFire(); // Включить быструю стрельбу
                    break;
                case BonusType.Health:
                    playerShooting.AddHeal(20); // Восстановить здоровье
                    break;
                case BonusType.Speed:
                    playerShooting.AddSpeed(2f); // Увеличить скорость
                    break;
                case BonusType.NewWeapon:
                    playerShooting.AddWing(); //Добавляем крыло
                    break;
            }
        }
    }
}
