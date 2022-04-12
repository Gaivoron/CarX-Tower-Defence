using System;
using TowerDefence.Monsters;
using TowerDefence.Towers;
using UnityEngine;

namespace TowerDefence
{
    public sealed class GameplayInitializer : MonoBehaviour, IGameplayData
    {
        public event Action Defeated;
        public event Action<int> LiveForceChanged;

        [SerializeField]
        private GameplayConfig m_config;

        [Space]
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("_towers")]
        private Tower[] m_towers;

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("_spawners")]
        private MonsterSpawner[] m_spawners;

        private int m_liveForce;

        public int LiveForce
        {
            get => m_liveForce;
            set
            {
                if (m_liveForce == value)
                {
                    return;
                }

                m_liveForce = value;
                LiveForceChanged?.Invoke(m_liveForce);
                if (m_liveForce <= 0)
                {
                    Defeated?.Invoke();
                }
            }
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
            m_liveForce = config.LiveForce;

            MonsterRoster = new MonsterRoster(m_spawners);
            foreach (var spawner in m_spawners)
            {
                spawner.Initialize(this);
                spawner.StartWave(config.Wave);
            }
            foreach (var tower in m_towers)
            {
                tower.Initialize(this);
            }

            new GameFinisher(this);
        }
    }
}