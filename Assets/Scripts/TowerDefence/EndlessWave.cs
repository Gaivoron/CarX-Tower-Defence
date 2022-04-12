using System.Collections.Generic;

namespace TowerDefence
{
    //TODO - should be serializable.
    public sealed class EndlessWave : IWave
    {
        //TODO - should provide finite wave of monsters.
        IEnumerable<IWaveInstruction> IWave.Instructions
        {
            get
            {
                var instruction = new WaveInstruction
                {
                    Delay = 0,
                    Archetype = MonsterType.Minion,
                };
                yield return instruction;

                instruction.Delay = 2f;
                while (true)
                {
                    yield return instruction;
                }
            }
        }
    }
}