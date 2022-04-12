using TowerDefence.Monsters;
using UnityEngine;

namespace TowerDefence.Projectiles
{
    public sealed class GuidedProjectile : MonoBehaviour
	{
		[SerializeField]
		private float m_speed = 0.2f;

		private ITarget m_target;

		public void SetTarget(ITarget target)
		{
			if (m_target != null)
			{
				m_target.Released -= OnTargetReleased;
			}

			m_target = target;
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

			var distance = m_target.Mover.Position - transform.position;
			var estimatedTime = distance.magnitude / m_speed;
			var predictedPosition = m_target.Mover.PredictPosition(estimatedTime);
			var direction = predictedPosition - transform.position;
			distance = m_speed * Time.deltaTime * direction.normalized;

			transform.Translate(distance);
		}
	}
}