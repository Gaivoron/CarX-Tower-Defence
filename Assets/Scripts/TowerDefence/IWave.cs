using System.Collections.Generic;

namespace TowerDefence
{
    public interface IWave
    {
        IEnumerable<IWaveInstruction> Instructions { get; }
    }
}