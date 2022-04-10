using UnityEngine;

namespace TowerDefence.Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour
	{
		public abstract ICalibration Target(Vector3 lasersight, float maxTime);
	}
}