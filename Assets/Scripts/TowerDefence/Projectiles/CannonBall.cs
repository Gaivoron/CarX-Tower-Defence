﻿using UnityEngine;

namespace TowerDefence.Projectiles
{
    public sealed class CannonBall : MonoBehaviour
	{
		[SerializeField]
		private float m_speed = 0.2f;

		public float Speed => m_speed;

		private void Update()
		{
			var translation = transform.forward * m_speed * Time.deltaTime;
			transform.position += translation;
		}

        private void OnDrawGizmos()
        {
			var color = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, transform.forward * 100);
			Gizmos.color = color;
		}
    }
}