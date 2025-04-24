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

        void Die()
        {
            Destroy(gameObject);
            // Здесь позже добавим эффекты
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player.PlayerShooting.health -= hp / 10 * 5;
                Die();
            }
        }
    }
}