using System;
using UnityEngine;

namespace Project.Scripts.Config
{
    [CreateAssetMenu(fileName = "CoinConfig", menuName = "Config/CoinConfig")]
    public class CoinConfig: ScriptableObject
    {
        [SerializeField] private CoinStat _coinStat;
     
        public CoinStat CoinStat => _coinStat;
    }
    
    //ToDo _bonusValuePerWave doesn't used in the project
    [Serializable]
    public struct CoinStat
    {
        [Header("Values")]
        [SerializeField] private int _value;
        [SerializeField] private float _pickupRange;
        [Header("Rules")]
        [SerializeField] private int _bonusValuePerWave;
        
        public int Value => _value;
        public float PickupRange => _pickupRange;
        
        public void MultiplyByWave(int wave)
        {
            _value += _bonusValuePerWave * wave;
        }
        
        public static CoinStat operator +(CoinStat a, CoinStat b)
        {
            return new CoinStat
            {
                _value = a._value + b._value,
                _pickupRange = a._pickupRange + b._pickupRange
            };
        }
    }
}