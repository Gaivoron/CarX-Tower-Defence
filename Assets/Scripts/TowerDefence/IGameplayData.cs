using TowerDefence.Monsters;

namespace TowerDefence
{
    public interface IGameplayData
	{
		int LiveForce { get; set; }

		IMonsterRoster MonsterRoster { get; }
	}
}