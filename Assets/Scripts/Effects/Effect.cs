using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Effects
{
    public class Effect : MonoBehaviour
    {
        public Earth earth;
        public Player player;
        public Enemy enemy;

        void Awake()
        {
            earth = new Earth("Sprites/Effects/Earth");
            player = new Player("Sprites/Effects/Player", this);
            enemy = new Enemy("Sprites/Effects/Enemy", this);
        }

        public class Earth
        {
            private List<GameObject> damageEffects = new List<GameObject>();

            public Earth(string path)
            {
                LoadEffects(path);
            }

            void LoadEffects(string path)
            {
                GameObject[] loaded = Resources.LoadAll<GameObject>(path);
                damageEffects.AddRange(loaded);
            }

            public void SpawnDamage(Vector2 position)
            {
                position = new Vector2(position.x, position.y - 0.65f);
                if (damageEffects.Count == 0) return;
                GameObject prefab = damageEffects[Random.Range(0, damageEffects.Count)];
                Instantiate(prefab, position, Quaternion.identity);
            }
        }

        public class Player
        {
            private List<GameObject> playerEffects = new List<GameObject>();
            private Effect parent;

            public Player(string path, Effect parent)
            {
                this.parent = parent;
                LoadEffects(path);
            }

            void LoadEffects(string path)
            {
                GameObject[] loaded = Resources.LoadAll<GameObject>(path);
                playerEffects.AddRange(loaded);
            }

            public void SpawnDamage(Vector2 hitPoint, GameObject root)
            {
                if (playerEffects.Count == 0) return;

                GameObject prefab = playerEffects.Find(p => p != null && p.name == "damage");
                if (prefab == null)
                {
                    Debug.LogWarning("Effect prefab named 'damage' not found.");
                    return;
                }

                // Находим все Collider2D в иерархии
                Collider2D[] colliders = root.GetComponentsInChildren<Collider2D>();

                if (colliders.Length == 0)
                {
                    Debug.LogWarning("No colliders found in root object.");
                    return;
                }

                // Ищем ближайший коллайдер к точке попадания
                Collider2D nearestCollider = colliders
                    .OrderBy(c => Vector2.Distance(c.bounds.ClosestPoint(hitPoint), hitPoint))
                    .FirstOrDefault();

                if (nearestCollider == null) return;

                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
                Vector2 center = nearestCollider.bounds.center;
                Vector2 direction = (center - hitPoint).normalized;

                Vector2 testPoint = hitPoint;
                float offset = 0.2f;

                for (int i = 0; i < 10; i++)
                {
                    if (nearestCollider.OverlapPoint(testPoint))
                        break;

                    testPoint += direction * offset;
                }

                Instantiate(prefab, testPoint, rotation, nearestCollider.transform);
            }
        }

        public class Enemy
        {
            private Dictionary<string, GameObject> enemyEffects = new Dictionary<string, GameObject>();
            private Effect parent;

            public Enemy(string path, Effect parent)
            {
                this.parent = parent;
                LoadEffects(path);
            }

            void LoadEffects(string path)
            {
                GameObject[] loaded = Resources.LoadAll<GameObject>(path);
                foreach (var obj in loaded)
                {
                    if (!enemyEffects.ContainsKey(obj.name))
                        enemyEffects.Add(obj.name, obj);
                }
            }

            public void SpawnIskraEffect(GameObject other, Vector2 position, float z)
            {
                if (enemyEffects.Count == 0) return;

                List<GameObject> values = new List<GameObject>(enemyEffects.Values);
                GameObject prefab = values[Random.Range(0, values.Count)];

                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));

                // Настоящий центр родителя
                Vector3 centerPosition = other.transform.position;

                // Смещаем ближе к центру относительно исходной позиции
                Vector3 directionToCenter = (centerPosition - (Vector3)position).normalized;
                float offsetDistance = 0.9f; // Насколько ближе к центру (можешь менять число)

                Vector3 adjustedPosition = (Vector3)position + directionToCenter * offsetDistance;

                GameObject spawned = Instantiate(prefab, adjustedPosition, rotation, other.transform);

                parent.Destroy_Effect(spawned, 1f);
            }
        }

        public void Destroy_Effect(GameObject effect, float timer)
        {
            StartCoroutine(Destroy(effect, timer));
        }
        
        private IEnumerator Destroy(GameObject effect, float timer)
        {
            SpriteRenderer spriteRenderer = effect.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                yield return new WaitForSeconds(timer);
                Destroy(effect);
                yield break;
            }

            float elapsed = 0f;
            Color originalColor = spriteRenderer.color;

            while (elapsed < timer)
            {
                if (effect != null)
                {
                    yield break;
                }
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / timer);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            
            Destroy(effect);
        }
    }
}
