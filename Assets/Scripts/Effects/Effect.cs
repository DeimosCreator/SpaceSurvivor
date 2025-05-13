using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
            player = new Player("Sprites/Effects/Player");
            enemy = new Enemy("Sprites/Effects/Enemy");
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
            private List<GameObject> playerEffects = new();

            public Player(string path)
            {
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
            private Dictionary<string, GameObject> enemyEffects = new();

            public Enemy(string path)
            {
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
            
            public void SpawnIskraEffect(GameObject other, Vector2 position)
            {
                if (enemyEffects.Count == 0) return;

                List<GameObject> values = new List<GameObject>(enemyEffects.Values);
                GameObject prefab = values[Random.Range(0, values.Count)];

                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));

                Vector3 centerPosition = other.transform.position;

                Vector3 directionToCenter = (centerPosition - (Vector3)position).normalized;
                float offsetDistance = 0.9f;

                Vector3 adjustedPosition = (Vector3)position + directionToCenter * offsetDistance;

                Instantiate(prefab, adjustedPosition, rotation, other.transform);
            }
            
            public void SpawnDieEffect(GameObject enemyObj)
            {
                Debug.Log("SpawnDieEffect...");
                SpriteRenderer sr = enemyObj.GetComponent<SpriteRenderer>();
                if (sr == null || sr.sprite == null)
                {
                    Debug.LogWarning("Enemy object has no SpriteRenderer or sprite.");
                    return;
                }

                Sprite originalSprite = sr.sprite;
                Texture2D texture = originalSprite.texture;

                if (!texture.isReadable)
                {
                    Debug.LogWarning("Texture is not readable. Enable 'Read/Write' in import settings.");
                    return;
                }

                Rect spriteRect = originalSprite.rect;
                int x = Mathf.RoundToInt(spriteRect.x);
                int y = Mathf.RoundToInt(spriteRect.y);
                int totalWidth = Mathf.RoundToInt(spriteRect.width);
                int totalHeight = Mathf.RoundToInt(spriteRect.height);

                int partsCount = Random.Range(4, 7);
                int basePartWidth = totalWidth / partsCount;
                int extraPixels = totalWidth % partsCount;

                int accumulatedX = 0;

                for (int i = 0; i < partsCount; i++)
                {
                    int partWidth = basePartWidth + (i < extraPixels ? 1 : 0);
                    int pixelX = x + accumulatedX;

                    if (pixelX + partWidth > texture.width)
                        partWidth = texture.width - pixelX;

                    Color[] pixels = texture.GetPixels(pixelX, y, partWidth, totalHeight);
                    Texture2D partTexture = new Texture2D(partWidth, totalHeight, TextureFormat.ARGB32, false);
                    partTexture.SetPixels(pixels);
                    partTexture.Apply();

                    Sprite partSprite = Sprite.Create(
                        partTexture,
                        new Rect(0, 0, partWidth, totalHeight),
                        new Vector2(0.5f, 0.5f),
                        originalSprite.pixelsPerUnit
                    );

                    GameObject partObj = new GameObject($"EnemyPart_{i}");
                    partObj.transform.position = enemyObj.transform.position;
                    partObj.transform.localScale = enemyObj.transform.localScale;

                    SpriteRenderer partSr = partObj.AddComponent<SpriteRenderer>();
                    partSr.sprite = partSprite;
                    partSr.sortingLayerID = sr.sortingLayerID;
                    partSr.sortingOrder = sr.sortingOrder;

                    Rigidbody2D rb = partObj.AddComponent<Rigidbody2D>();
                    rb.gravityScale = 0.5f;
                    rb.AddForce(Random.insideUnitCircle.normalized * Random.Range(2f, 4f), ForceMode2D.Impulse);
                    rb.AddTorque(Random.Range(-100f, 100f));

                    Effect effect = FindObjectOfType<Effect>();
                    if (effect != null)
                    {
                        effect.Destroy_Effect(partObj, 1.5f);
                    }

                    accumulatedX += partWidth;
                }

                Destroy(enemyObj);
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
                if (effect == null)
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
