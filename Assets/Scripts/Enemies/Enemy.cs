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
                Die("Bullet");
            }
        }

        void Die(string name, int damage = 0)
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
                effect.earth.SpawnDamage(gameObject.transform.position);
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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player.PlayerShooting.health -= hp / 10 * 5;
                
                Vector2 hitPoint = transform.position;
                Effects.Effect effect = FindObjectOfType<Effects.Effect>();
                effect.player.SpawnDamage(hitPoint, other.gameObject);
                Die("Player");
            }

            if (other.gameObject.CompareTag("Planet"))
            {
                // Когда враг врезается в планету
                int damageToPlanet = hp / 2; 
                Die("Planet", damageToPlanet);
            }
        }
    }
}