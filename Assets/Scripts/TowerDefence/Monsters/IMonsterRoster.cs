using System;
using System.Collections.Generic;

namespace TowerDefence.Monsters
{
    public interface IMonsterRoster
	{
		event Action<ITarget> MonsterReachedFinalDestination;

		public IEnumerable<ITarget> Monsters { get; }
	}
}