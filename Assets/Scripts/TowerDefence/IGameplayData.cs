using System;
using TowerDefence.Monsters;

namespace TowerDefence
{
    public interface IGameplayData
	{
		event Action Defeated;
		event Action<int> LiveForceChanged;

		int LiveForce { get; set; }

		IMonsterRoster MonsterRoster { get; }
	}
}