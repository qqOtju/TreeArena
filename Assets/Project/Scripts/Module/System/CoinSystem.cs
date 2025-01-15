using System;

namespace Project.Scripts.Module.System
{
    public class CoinSystem
    {
        
        private int _currentGold;
        
        public int CurrentGold
        {
            get => _currentGold;
            set
            {
                if(value < 0)
                    _currentGold = 0;
                else if(value > 999999)
                    _currentGold = 999999;
                else
                    _currentGold = value;
                OnGoldChanged?.Invoke(_currentGold);
            }
            
        }
        
        public event Action<int> OnGoldChanged;
        
        public CoinSystem(int amount)
        {
            CurrentGold = amount;
        }
    }
}