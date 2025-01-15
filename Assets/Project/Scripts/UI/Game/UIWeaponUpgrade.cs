using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Game
{
    public class UIWeaponUpgrade: MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _upgradeButton;
        
        public Button UpgradeButton => _upgradeButton;
        
        public void Initialize(string upgradeName, string value, 
            string price, Sprite icon)
        {
            _nameText.text = upgradeName;
            _valueText.text = value;
            _priceText.text = price;
            _icon.sprite = icon;
        }
    }
}