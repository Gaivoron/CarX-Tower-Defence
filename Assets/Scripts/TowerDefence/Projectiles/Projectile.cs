using UnityEngine;

namespace TowerDefence.Projectiles
{
    public abstract class Projectile : MonoBehaviour
	{
		public abstract ICalibration Target(Vector3 lasersight);
	}
}