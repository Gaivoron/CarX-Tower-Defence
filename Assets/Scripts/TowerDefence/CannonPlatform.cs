using TowerDefence.Monsters;
using UnityEngine;

namespace TowerDefence
{
	public sealed class CannonPlatform : WeaponPlatform
	{
		public GameObject m_projectilePrefab;
		public Transform m_shootPoint;

		[Space]
		[Header("Rotation")]
		[SerializeField]
		private Transform m_yRotor;
		[SerializeField]
		private Transform m_xRotor;

		protected override void Shoot(IMonster target)
        {
			Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
		}
	}
}