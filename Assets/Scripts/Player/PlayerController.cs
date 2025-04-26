using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 100f;

        private Rigidbody2D rb;
        private Vector2 moveDirection;
        private float rotationInput;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            moveDirection = Vector2.zero;
            rotationInput = 0f;

            // Движение только через W A S D
            if (Input.GetKey(KeyCode.W))
                moveDirection.y = 1f;
            if (Input.GetKey(KeyCode.S))
                moveDirection.y = -1f;
            if (Input.GetKey(KeyCode.D))
                moveDirection.x = 1f;
            if (Input.GetKey(KeyCode.A))
                moveDirection.x = -1f;

            // Поворот только через стрелочки
            if (Input.GetKey(KeyCode.LeftArrow))
                rotationInput = 1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                rotationInput = -1f;
        }

        void FixedUpdate()
        {
            // Поворот
            rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime);

            // Перемещение относительно поворота
            if (transform != null)
            {
                var transform1 = transform;
                Vector2 move = transform1.up * moveDirection.y + transform1.right * moveDirection.x;
                rb.MovePosition(rb.position + move.normalized * (moveSpeed * Time.fixedDeltaTime));
            }
        }
    }
}