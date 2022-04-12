using UnityEngine;

namespace TowerDefence.Projectiles
{
    public interface ICalibration
	{
		float Time { get; }
		Vector3 Orientation { get; }
	}
}