using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence
{
    public sealed class MagicPlatform : WeaponPlatform
	{
		[SerializeField]
		private GameObject m_projectilePrefab;

        protected override void Shoot(IMonster target)
        {
			if (m_projectilePrefab == null)
				return;

			var projectile = Instantiate(m_projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
			var projectileBeh = projectile.GetComponent<GuidedProjectile>();
			projectileBeh.Target = target;
		}
    }
}