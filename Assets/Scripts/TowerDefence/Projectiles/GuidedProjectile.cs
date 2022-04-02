using TowerDefence.Monsters;
using UnityEngine;

namespace TowerDefence.Projectiles
{
	public sealed class GuidedProjectile : MonoBehaviour
	{
		[SerializeField]
		private float m_speed = 0.2f;

		public IMonster Target
		{
			private get;
			set;
		}

		private void Update()
		{
			if (Target == null)
			{
				Destroy(gameObject);
				return;
			}

			var translation = Target.Position - transform.position;
			if (translation.magnitude > m_speed)
			{
				translation = translation.normalized * m_speed;
			}
			transform.Translate(translation);
		}
	}
}