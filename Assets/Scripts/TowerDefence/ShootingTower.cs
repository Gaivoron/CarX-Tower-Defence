using UnityEngine;

namespace TowerDefence
{
    public abstract class ShootingTower : MonoBehaviour
	{
		public float m_shootInterval = 0.5f;
		public float m_range = 4f;

		private float m_lastShotTime = -0.5f;

		protected abstract void Shoot(Monster target);

        private void Update()
        {
			if (m_lastShotTime + m_shootInterval > Time.time)
			{
				return;
			}

			foreach (var monster in FindObjectsOfType<Monster>())
			{
				if (Vector3.Distance(transform.position, monster.transform.position) > m_range)
					continue;

				// shot
				Shoot(monster);
				m_lastShotTime = Time.time;
				break;
			}
		}
    }
}