using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ButtonStart : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
