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

			public Vector3 Axis => m_axis;
			public Vector3 Forward => m_rotor.forward;
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
		}
	}
}