using TowerDefence.Monsters;
using UnityEngine;

namespace TowerDefence.Projectiles
{
    public sealed class GuidedProjectile : MonoBehaviour
	{
		[SerializeField]
		private float m_speed = 0.2f;

		private IMonster m_target;

		public void SetTarget(IMonster value)
		{
			if (m_target != null)
			{
				m_target.Released -= OnTargetReleased;
			}

			m_target = value;
			if (m_target != null)
			{
				m_target.Released += OnTargetReleased;
			}
		}

        private void OnTargetReleased()
        {
			m_target.Released -= OnTargetReleased;
			m_target = null;
		}

        private void Update()
		{
			if (m_target == null)
			{
				Destroy(gameObject);
				return;
			}

			var translation = m_target.Mover.Position - transform.position;
			translation = translation.normalized * m_speed * Time.deltaTime;

			transform.Translate(translation);
		}
	}
}