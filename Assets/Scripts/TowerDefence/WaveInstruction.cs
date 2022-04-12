namespace TowerDefence
{
    [System.Serializable]
    public struct WaveInstruction : IWaveInstruction
    {
        public float Delay
        {
            get;
            set;
        }

        public MonsterType Archetype
        {
            get;
            set;
        }
    }
}