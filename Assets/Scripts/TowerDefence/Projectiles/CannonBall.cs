using UnityEngine;

namespace TowerDefence.Projectiles
{
	public class CannonBall : MonoBehaviour
	{
		public float m_speed = 0.2f;

		private void Update()
		{
			var translation = transform.forward * m_speed;
			transform.Translate(translation);
		}
	}
}