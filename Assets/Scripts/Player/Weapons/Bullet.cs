using UnityEngine;

namespace Player.Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 5;

        void Update()
        {
            transform.Translate(Vector2.up * (speed * Time.deltaTime));
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<Enemies.Enemy>();
            if (enemy != null)
            {
                Effects.Effect effect = FindObjectOfType<Effects.Effect>();

                if (effect != null)
                {
                    effect.enemy.SpawnIskraEffect(other.gameObject, gameObject.transform.position, other.transform.rotation.z);
                }
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