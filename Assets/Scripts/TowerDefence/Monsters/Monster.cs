using System;
using UnityEngine;

namespace TowerDefence.Monsters
{
    //TODO - turn into an Object for ObjectPool.
    public sealed class Monster : MonoBehaviour, IMonster
	{
		public event Action Died;
		public event Action Released;

		const float m_reachDistance = 0.3f;

		public float m_speed = 0.1f;
		public int m_maxHP = 30;

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
					Died?.Invoke();
					//TODO - play death animation?
					Released?.Invoke();
				}
			}
		}

		public GameObject MoveTarget
		{
			get;
			set;
		}

        public Vector3 Position => transform.position;

        private void Start()
		{
			HP = m_maxHP;
		}

		private void Update()
		{
			if (MoveTarget == null)
				return;

			if (Vector3.Distance(transform.position, MoveTarget.transform.position) <= m_reachDistance)
			{
				Released?.Invoke();
				Destroy(gameObject);
				return;
			}

			var translation = MoveTarget.transform.position - transform.position;
			if (translation.magnitude > m_speed)
			{
				translation = translation.normalized * m_speed;
			}
			transform.Translate(translation);
		}
	}
}