using UnityEngine;

namespace TowerDefence.Monsters
{
    public sealed class LinearPath : MonoBehaviour, IPath
	{
		[SerializeField]
		private Transform m_moveTarget;

        private float? m_length;

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

        private Vector3 Distance => m_moveTarget.position - transform.position;

        public Vector3 GetPosition(float progress)
        {
            return transform.position + (progress / Length) * Distance;
        }
    }
}