using UnityEngine;

namespace TowerDefence
{
	//TODO - turn into an Object for ObjectPool.
	public sealed class Monster : MonoBehaviour
	{
		public GameObject m_moveTarget;
		public float m_speed = 0.1f;
		public int m_maxHP = 30;
		const float m_reachDistance = 0.3f;

		private int m_HP;

		public int HP
		{
			get => m_HP;
			set
			{
				m_HP = value;
				if (m_HP <= 0)
				{
					Destroy(gameObject);
				}
			}
		}

		private void Start()
		{
			HP = m_maxHP;
		}

		private void Update()
		{
			if (m_moveTarget == null)
				return;

			if (Vector3.Distance(transform.position, m_moveTarget.transform.position) <= m_reachDistance)
			{
				Destroy(gameObject);
				return;
			}

			var translation = m_moveTarget.transform.position - transform.position;
			if (translation.magnitude > m_speed)
			{
				translation = translation.normalized * m_speed;
			}
			transform.Translate(translation);
		}
	}
}