using UnityEngine;

namespace UI
{
    public class UIPanelManager : MonoBehaviour
    {
        public void SwitchPanel(GameObject newPanel, GameObject oldPanel)
        {
            var fadeOut = oldPanel.GetComponent<UIFade>();
            var fadeIn = newPanel.GetComponent<UIFade>();
            
            fadeOut.FadeOut(() =>
            {
                oldPanel.SetActive(false);
                newPanel.SetActive(true);
                if (fadeIn) fadeIn.FadeIn();
            });
        }
    }
}