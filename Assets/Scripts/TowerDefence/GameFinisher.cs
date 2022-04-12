using TowerDefence.Monsters;

namespace TowerDefence
{
    public sealed class GameFinisher
    {
        private readonly IGameplayData m_data;

        public GameFinisher(IGameplayData data)
        {
            m_data = data;
            data.MonsterRoster.MonsterReachedFinalDestination += OnMonsterEscaped;
        }

        private void OnMonsterEscaped(ITarget monster)
        {
            m_data.LiveForce -= GetThreat(monster);
            monster.Release();
        }

        private int GetThreat(ITarget monster)
        {
            //TODO - determine 'threat' value based on monster type and\or other parameters.
            return 1;
        }
    }
}