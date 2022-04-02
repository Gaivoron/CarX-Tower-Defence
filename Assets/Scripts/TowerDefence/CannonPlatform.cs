using TowerDefence.Monsters;
using UnityEngine;

namespace TowerDefence
{
	public sealed class CannonPlatform : WeaponPlatform
	{
		public GameObject m_projectilePrefab;
		public Transform m_shootPoint;

        protected override void Shoot(IMonster target)
        {
			Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
		}
	}
}