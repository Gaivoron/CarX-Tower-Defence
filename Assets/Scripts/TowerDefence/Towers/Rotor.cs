using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed partial class CannonPlatform
    {
        [System.Serializable]
		private sealed class Rotor
		{
			[SerializeField]
			private Transform m_rotor;
			[SerializeField]
			private float m_speed = 0.5f;
			[SerializeField]
			private Vector3 m_axis;

			private float m_angle = 0f;

			public float Speed => m_speed;

			public float Angle
			{
				get => m_angle;
                set
				{
					if (value != m_angle)
					{
						m_rotor.Rotate(m_axis, value - m_angle);
						m_angle = value;
					}
				}
			}

            public float GetAngle(Vector3 direction) => Vector3.SignedAngle(Vector3.ProjectOnPlane(m_rotor.forward, m_axis), Vector3.ProjectOnPlane(direction, m_axis), m_axis);
		}
	}
}