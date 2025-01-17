using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Menu.Options
{
    public class UIVolume: MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Slider _slider;
        
        private string _volumeName;
        
        public event Action<float> OnVolumeChanged;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float arg0)
        {
            _name.text = $"{_volumeName} %{arg0 * 100:F0}";
            OnVolumeChanged?.Invoke(arg0);
        }

        public void Initialize(string volumeName, float volumeValue)
        {
            _volumeName = volumeName;
            _slider.value = volumeValue;
            _name.text = $"{_volumeName} %{volumeValue * 100:F0}";
        }
    }
}