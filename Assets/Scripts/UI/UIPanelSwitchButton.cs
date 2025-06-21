using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UIPanelSwitchButton : MonoBehaviour
    {
        [Tooltip("Панель, которую нужно открыть")]
        public GameObject newPanel;

        [Tooltip("Текущая панель")]
        public GameObject oldPanel;

        private Button button;
        private UIPanelManager manager;

        private void Awake()
        {
            button = GetComponent<Button>();
            manager = FindObjectOfType<UIPanelManager>();

            if (manager == null)
            {
                Debug.LogError("UIPanelManager не найден на сцене.");
                return;
            }

            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            manager.SwitchPanel(newPanel, oldPanel);
        }
    }
}