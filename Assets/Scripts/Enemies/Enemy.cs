using UI;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public int hp = 10;

        public void TakeDamage(int amount)
        {
            hp -= amount;
            if (hp <= 0)
            {
                Die();
            }
        }

        void Die(string name = null, int damage = 0)
        {
            if (name == "Planet")
            {
                // Найти объект на сцене
                StatusPlanetBur statusPlanetBur = FindObjectOfType<StatusPlanetBur>();

                if (statusPlanetBur != null)
                {
                    statusPlanetBur.SetScale(damage); // Передаём урон
                }
            }

            Destroy(gameObject);
            // Здесь позже добавим эффекты
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Столкновение с: {other.gameObject.name}");

            if (other.gameObject.CompareTag("Player"))
            {
                Player.PlayerShooting.health -= hp / 10 * 5;
                Die();
            }

            if (other.gameObject.CompareTag("Planet"))
            {
                // Когда враг врезается в планету
                int damageToPlanet = hp / 2; // например половина его жизни как урон планете
                Die("Planet", damageToPlanet);
            }
        }
    }
}