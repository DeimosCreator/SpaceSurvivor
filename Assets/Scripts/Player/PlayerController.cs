using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;

        private Rigidbody2D rb;
        private Vector2 moveDirection;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = Input.GetAxis("Vertical");
        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }
}