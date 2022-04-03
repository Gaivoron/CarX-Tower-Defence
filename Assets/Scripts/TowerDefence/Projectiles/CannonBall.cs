using UnityEngine;

namespace TowerDefence.Projectiles
{
	public sealed class CannonBall : MonoBehaviour
	{
		public float m_speed = 0.2f;

		private void Update()
		{
			var translation = transform.forward * m_speed * Time.deltaTime;
			transform.Translate(translation);
		}
	}
}