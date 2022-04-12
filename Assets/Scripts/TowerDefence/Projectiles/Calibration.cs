using UnityEngine;

namespace TowerDefence.Projectiles
{
    internal struct Calibration : ICalibration
    {
        public float Time { get; set; }

        public Vector3 Orientation { get; set; }
	}
}