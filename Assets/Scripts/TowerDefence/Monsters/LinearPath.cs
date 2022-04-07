using UnityEngine;

namespace TowerDefence.Monsters
{
    public sealed class LinearPath : MonoBehaviour, IPath
	{
		[SerializeField]
		private Transform m_moveTarget;

        private float? m_length;
        private Vector3? m_direction;

        public float Length
        {
            get
            {
                if (!m_length.HasValue)
                {
                    m_length = Distance.magnitude;
                }

                return m_length.Value;
            }
        }

        private Vector3 Direction
        {
            get
            {
                if (!m_direction.HasValue)
                {
                    m_direction = Distance.normalized;
                }

                return m_direction.Value;
            }
        }

        private Vector3 Distance => m_moveTarget.position - transform.position;

        public Vector3 GetPosition(float progress)
        {
            //TODO - clamp result between 0 and Length?
            return transform.position + (progress / Length) * Distance;
        }
    }
}