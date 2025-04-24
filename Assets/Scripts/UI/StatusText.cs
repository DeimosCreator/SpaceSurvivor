using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatusText : MonoBehaviour
    {
        public Text health;
        public Text ammo;
        public Text time;
        public Text level;

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
