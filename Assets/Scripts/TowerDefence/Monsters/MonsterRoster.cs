using System;
using System.Collections.Generic;

namespace TowerDefence.Monsters
{
    public sealed class MonsterRoster : IMonsterRoster
	{
		public event Action<ITarget> MonsterReachedFinalDestination;

		private readonly IList<ITarget> m_monstersList = new List<ITarget>();

		IEnumerable<ITarget> IMonsterRoster.Monsters => m_monstersList;

		public MonsterRoster(IEnumerable<MonsterSpawner> spawners)
		{
			//TODO - cache all spawners to properly release them later?
			foreach (var spawner in spawners)
			{
				spawner.Spawned += OnMonsterSpawned;
			}
		}

        private void OnMonsterSpawned(ITarget monster)
        {
			m_monstersList.Add(monster);
			monster.Mover.FinishReached += OnFinishReached;
			monster.Released += OnReleased;

			void OnReleased()
			{
				monster.Mover.FinishReached -= OnFinishReached;
				monster.Released -= OnReleased;
				m_monstersList.Remove(monster);
			}

			void OnFinishReached()
			{
				MonsterReachedFinalDestination?.Invoke(monster);
			}
		}
    }
}