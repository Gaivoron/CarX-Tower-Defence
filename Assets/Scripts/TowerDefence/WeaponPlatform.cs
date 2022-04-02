using UnityEngine;
using System.Linq;
using TowerDefence.Monsters;
using System.Collections.Generic;

namespace TowerDefence
{
    public abstract class WeaponPlatform : MonoBehaviour, IPlatform
	{
		public float m_shootInterval = 0.5f;
		public float m_range = 4f;

		private float m_lastShotTime = -0.5f;
        private IGameplayData m_data;

		private IEnumerable<IMonster> Monsters => m_data.MonsterRoster.Monsters;

        public void Initialize(IGameplayData data)
        {
			m_data = data;
        }

        protected abstract void Shoot(IMonster target);

        private void Update()
        {
			//recharging shot
			if (m_lastShotTime + m_shootInterval > Time.time)
			{
				return;
			}

			//finding target
			var target = FindTarget();
			if (target != null)
			{
				Shoot(target);
				m_lastShotTime = Time.time;
			}
		}

		private IMonster FindTarget()
		{
			//TODO - get list of all active monsters.
			return Monsters.FirstOrDefault(IsValidTarget);
		}

        private bool IsValidTarget(IMonster target)
        {
			var distance = transform.position - target.Position;
			return distance.sqrMagnitude <= m_range * m_range;
		}
    }
}