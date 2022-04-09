using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed partial class CannonPlatform
    {
        private interface ITargetData
		{
			Vector3 Point { get; }

			float HitTime { get; }
			float FlightTime { get; }
			float RotationTime { get; }
			Vector3 Angles { get; }
		}
	}
}