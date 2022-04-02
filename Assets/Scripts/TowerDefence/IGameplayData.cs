using TowerDefence.Monsters;

namespace TowerDefence
{

    public interface IGameplayData
	{
		IMonsterRoster MonsterRoster { get; }
	}
}