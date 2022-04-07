using TowerDefence.Monsters;
using TowerDefence.Towers;
using UnityEngine;

namespace TowerDefence
{
    public sealed class GameplayInitializer : MonoBehaviour, IGameplayData
    {
        [SerializeField]
        private GameplayConfig m_config;

        [Space]
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("_towers")]
        private Tower[] m_towers;

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("_spawners")]
        private MonsterSpawner[] m_spawners;

        public int LiveForce
        {
            get;
            set;
        }

        public IMonsterRoster MonsterRoster
        {
            get;
            private set;
        }

        private void Awake()
        {
            Initialize(m_config);
        }

        //TODO - should be public.
        private void Initialize(GameplayConfig config)
        {
            LiveForce = config.LiveForce;

            MonsterRoster = new MonsterRoster(m_spawners);
            foreach (var tower in m_towers)
            {
                tower.Initialize(this);
            }

            new GameFinisher(this);
        }
    }
}