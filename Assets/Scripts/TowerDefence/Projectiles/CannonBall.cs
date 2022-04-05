using UnityEngine;

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
			transform.Translate(translation);
		}
	}
}