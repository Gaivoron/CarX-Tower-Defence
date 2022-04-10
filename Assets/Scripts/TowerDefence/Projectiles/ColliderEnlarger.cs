using UnityEngine;

namespace TowerDefence.Projectiles
{
    public sealed class ColliderEnlarger : MonoBehaviour
	{
		[SerializeField]
		private float m_factor = 0.1f;
		[SerializeField]
		private SphereCollider m_collider;

		private float m_originalRadius;


		private void Awake()
        {
			m_originalRadius = m_collider.radius;
		}

        private void Update()
        {
			m_collider.radius += m_originalRadius * m_factor * Time.deltaTime;
        }
    }
}