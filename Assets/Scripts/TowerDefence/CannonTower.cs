using UnityEngine;

namespace TowerDefence
{
	public sealed class CannonTower : ShootingTower
	{
		public GameObject m_projectilePrefab;
		public Transform m_shootPoint;

        protected override void Shoot(Monster target)
        {
			Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
		}
	}
}