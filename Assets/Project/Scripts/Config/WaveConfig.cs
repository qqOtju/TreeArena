using Project.Scripts.GameLogic.Wave;
using UnityEngine;

namespace Project.Scripts.Config
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Config/WaveConfig")]
    public class WaveConfig: ScriptableObject
    {
        [SerializeField] private WaveContent[] _spawns;
        
        public WaveContent[] Spawns => _spawns;
    }
}