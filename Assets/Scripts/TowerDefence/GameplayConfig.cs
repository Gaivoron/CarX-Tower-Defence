using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu(menuName = "TowerDefence/GameplayConfig")]
    public sealed class GameplayConfig : ScriptableObject
    {
        [SerializeField]
        private int m_liveForce = 10;

        public int LiveForce => m_liveForce;
    }
}