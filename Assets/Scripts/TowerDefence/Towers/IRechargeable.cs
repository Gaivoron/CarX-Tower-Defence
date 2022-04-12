using System;

namespace TowerDefence.Towers
{
    public interface IRechargeable
	{
		event Action<float, float> Recharged;
	}
}