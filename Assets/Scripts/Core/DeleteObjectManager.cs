using UnityEngine;

namespace Core
{
    public class DeleteObjectManager : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(other.gameObject);
        }
    }
}