using System.Collections.Generic;

namespace TowerDefence.Monsters
{
    public sealed class MonsterRoster : IMonsterRoster
	{
		private readonly IList<IMonster> m_monstersList = new List<IMonster>();

		IEnumerable<IMonster> IMonsterRoster.Monsters => m_monstersList;

		public MonsterRoster(IEnumerable<MonsterSpawner> spawners)
		{
			//TODO - cache all spawners to properly release them later?
			foreach (var spawner in spawners)
			{
				spawner.Spawned += OnMonsterSpawned;
			}
		}

        private void OnMonsterSpawned(IMonster monster)
        {
			m_monstersList.Add(monster);
			monster.Released += OnReleased;

			void OnReleased()
			{
				monster.Released -= OnReleased;
				m_monstersList.Remove(monster);
			}
		}
    }
}