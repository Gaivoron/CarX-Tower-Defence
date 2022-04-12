using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu(menuName = "TowerDefence/GameplayConfig")]
    public sealed class GameplayConfig : ScriptableObject
    {
        [SerializeField]
        private int m_liveForce = 10;

        //TODO - should have various serialized waves for multiple spawnpoints.
        private IWave m_wave;

        public int LiveForce => m_liveForce;

        public IWave Wave
        {
            get
            {
                if (m_wave == null)
                {
                    m_wave = new EndlessWave();
                }

                return m_wave;
            }
        }
    }
}