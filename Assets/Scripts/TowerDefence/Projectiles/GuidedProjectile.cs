using UnityEngine;

namespace TowerDefence.Projectiles
{
	public sealed class GuidedProjectile : MonoBehaviour
	{
		public GameObject m_target;
		public float m_speed = 0.2f;

		private void Update()
		{
			if (m_target == null)
			{
				Destroy(gameObject);
				return;
			}

			var translation = m_target.transform.position - transform.position;
			if (translation.magnitude > m_speed)
			{
				translation = translation.normalized * m_speed;
			}
			transform.Translate(translation);
		}
	}
}