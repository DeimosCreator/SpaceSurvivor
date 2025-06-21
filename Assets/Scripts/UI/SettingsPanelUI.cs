using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class SettingsPanelUI : MonoBehaviour
    {
        [Header("Audio")]
        public Slider musicSlider;
        public Slider sfxSlider;
        public TextMeshProUGUI musicValueText;
        public TextMeshProUGUI sfxValueText;

        [Header("Quality")]
        public Slider qualitySlider;
        public TextMeshProUGUI qualityLabel;

        private void Start()
        {
            // Загружаем настройки (сохраняем от 0 до 100)
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 100f);
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 100f);
            int quality = PlayerPrefs.GetInt("QualityLevel", 1);

            musicSlider.value = musicVolume;
            sfxSlider.value = sfxVolume;
            qualitySlider.value = quality;

            UpdateMusicText(musicVolume);
            UpdateSfxText(sfxVolume);
            UpdateQualityLabel(quality);

            // Назначаем слушателей
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            qualitySlider.onValueChanged.AddListener(SetQuality);
        }

        private void SetMusicVolume(float value)
        {
            //здесь будет дальнейшая логика сохранения настроек
            UpdateMusicText(value);
        }

        private void SetSfxVolume(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            UpdateSfxText(value);
            // Ты сам используешь значение где нужно
        }

        private void SetQuality(float value)
        {
            int level = Mathf.RoundToInt(value);
            QualitySettings.SetQualityLevel(level, true);
            PlayerPrefs.SetInt("QualityLevel", level);
            UpdateQualityLabel(level);
        }

        private void UpdateMusicText(float value)
        {
            musicValueText.text = Mathf.RoundToInt(value).ToString();
        }

        private void UpdateSfxText(float value)
        {
            sfxValueText.text = Mathf.RoundToInt(value).ToString();
        }

        private void UpdateQualityLabel(int level)
        {
            switch (level)
            {
                case 0: qualityLabel.text = "Низкое"; break;
                case 1: qualityLabel.text = "Среднее"; break;
                case 2: qualityLabel.text = "Высокое"; break;
                default: qualityLabel.text = $"Уровень {level}"; break;
            }
        }
    }
}
