using Project.Scripts.Config;
using Project.Scripts.DesignPattern.Factory;
using Project.Scripts.GameLogic.Dropdown;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory
{
    public class CoinFactory: ObjectFactory<Coin>
    {
        private const int BasePoolSize = 10;
        
        private readonly CoinConfig _coinConfig;
        private readonly DiContainer _diContainer;
        
        public CoinFactory(Coin prefab, Transform container, 
            CoinConfig config, DiContainer diContainer) : base(prefab, container)
        {
            Pool.Initialize(BasePoolSize);
            _coinConfig = config;
            _diContainer = diContainer;            
        }

        public override Coin Create()
        {
            var coin = Pool.Get();
            coin.Initialize(_coinConfig.CoinStat);
            _diContainer.Inject(coin);
            coin.OnPlayerHit += Release;
            return coin;
        }

        public override void Release(Coin obj)
        {
            obj.OnPlayerHit -= Release;
            Pool.Release(obj);
        }
    }
}