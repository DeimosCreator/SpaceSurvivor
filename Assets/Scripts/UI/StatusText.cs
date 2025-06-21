using System;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class StatusText : MonoBehaviour
    {
        public TextMeshProUGUI health;
        public TextMeshProUGUI ammo;
        public TextMeshProUGUI time;
        public TextMeshProUGUI level;

        public Player.PlayerShooting playerShooting;
        public Core.LevelManager levelManager;

        private void Update()
        {
            health.text = playerShooting.GetHealth().ToString();
            ammo.text = playerShooting.GetAmmo().ToString();
            time.text = levelManager.RemainingTime().ToString(CultureInfo.InvariantCulture);
            level.text = "Уровень " + levelManager.CurrentLevel().ToString();
        }
    }
}
