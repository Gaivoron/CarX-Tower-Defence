using System;
using System.Collections.Generic;

namespace TowerDefence.Monsters
{
    public interface IMonsterRoster
	{
		event Action<IMonster> MonsterReachedFinalDestination;

		public IEnumerable<IMonster> Monsters { get; }
	}
}