using UI;
using UnityEngine;

namespace Obstacles
{
    public class Obstacle: MonoBehaviour
    {
        public int hp = 20;
        
        public void TakeDamage()
        {
            Die("Bullet");
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
                Effects.Effect effect = FindObjectOfType<Effects.Effect>();
                effect.earth.SpawnDamage(gameObject.transform.position, damage, statusPlanetBur.currentAmount);
                effect.enemy.SpawnDieEffect(gameObject);
            }
            
            if (name == "Player")
            {
                Effects.Effect effect = FindObjectOfType<Effects.Effect>();
                effect.enemy.SpawnDieEffect(gameObject);
            }

            if (name == "Bullet")
            {
                Effects.Effect effect = FindObjectOfType<Effects.Effect>();
                effect.enemy.SpawnDieEffect(gameObject);
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player.PlayerShooting.health -= hp / 10 * 5;
                Die("Player");
            }

            if (other.gameObject.CompareTag("Planet"))
            {
                int damageToPlanet = hp / 2;
                Die("Planet", damageToPlanet);
            }
        }
    }
}