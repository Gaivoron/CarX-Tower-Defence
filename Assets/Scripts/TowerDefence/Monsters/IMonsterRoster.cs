using System.Collections.Generic;

namespace TowerDefence.Monsters
{
    public interface IMonsterRoster
	{
		public IEnumerable<IMonster> Monsters { get; }
	}
}