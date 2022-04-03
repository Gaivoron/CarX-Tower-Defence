using TowerDefence.Monsters;
using UnityEngine;

namespace TowerDefence
{
    public sealed class Gameplay : MonoBehaviour, IGameplayData
    {
        [SerializeField]
        private Tower[] _towers;

        [SerializeField]
        private MonsterSpawner[] _spawners;

        public IMonsterRoster MonsterRoster
        {
            get;
            private set;
        }

        private void Awake()
        {
            //TODO - should be actually initialized with some type of config.
            MonsterRoster = new MonsterRoster(_spawners);
            foreach (var tower in _towers)
            {
                tower.Initialize(this);
            }
        }
    }
}