using UnityEngine;

namespace Enemies
{
    public class EnemyMover : MonoBehaviour
    {
        public float speed = 2f;
        
        void Start()
        {
            speed = Random.Range(1.5f, 3.5f);
        }

        void Update()
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
}