using UnityEngine;

namespace Player.Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 10;

        void Update()
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<Enemies.Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }

            var obstacle = other.GetComponent<Obstacles.Obstacle>();
            if (obstacle != null)
            {
                obstacle.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}