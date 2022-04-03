using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence
{
    public sealed class MagicPlatform : WeaponPlatform
	{
		[SerializeField]
		private GuidedProjectile m_projectilePrefab;

		[SerializeField]
		private Transform m_spawnPoint;

        protected override void Shoot(IMonster target)
        {
			if (m_projectilePrefab == null)
			{
				return;
			}

			var projectile = Instantiate(m_projectilePrefab, m_spawnPoint.position, Quaternion.identity);
			projectile.SetTarget(target);
		}
    }
}