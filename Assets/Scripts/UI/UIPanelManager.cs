using UnityEngine;

namespace UI
{
    public class UIPanelManager : MonoBehaviour
    {
        public void SwitchPanel(GameObject newPanel, GameObject oldPanel)
        {
            var fadeOut = oldPanel.GetComponent<UIFade>();
            var fadeIn = newPanel.GetComponent<UIFade>();
            var ob = FindObjectsOfType<ButtonHoverScaler>();
            foreach (var o in ob)
            {
                o.FadeOut();
            }
            fadeOut.FadeOut(() =>
            {
                oldPanel.SetActive(false);
                newPanel.SetActive(true);
                fadeIn.FadeIn();
            });
        }
    }
}