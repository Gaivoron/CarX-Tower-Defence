namespace TowerDefence
{
    public interface IWaveInstruction
    {
        float Delay { get; }

        MonsterType Archetype { get; }
    }
}