using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed partial class CannonPlatform
    {
        private struct TargetData : ITargetData
		{
			public Vector3 Point { get; set; }
			public float HitTime { get; set; }
			public float FlightTime { get; set; }
			public float RotationTime { get; set; }
            public Vector3 Angles { get; set; }

            public override string ToString()
            {
                return $"{nameof(Point)} = {Point}, {nameof(Angles)} = {Angles}";
            }
        }
	}
}