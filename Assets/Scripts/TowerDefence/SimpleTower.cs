using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence
{
    public sealed class SimpleTower : ShootingTower
	{
		[SerializeField]

		private GameObject m_projectilePrefab;

        protected override void Shoot(Monster target)
        {
			if (m_projectilePrefab == null)
				return;

			var projectile = Instantiate(m_projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
			var projectileBeh = projectile.GetComponent<GuidedProjectile>();
			projectileBeh.m_target = target.gameObject;
		}
    }
}