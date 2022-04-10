using UnityEngine;
using Shared.Mathematics;

namespace TowerDefence.Projectiles
{
    public sealed class ArtilleryShell : ProjectileBase
	{
		[SerializeField]
		private float m_speed = 40f;
		[SerializeField]
		private float m_forceY = -9f;

        private Vector3 m_start;
        private Vector3 m_forces;
        private float m_time;

        //TODO - pass maxT as well
        public override ICalibration Target(Vector3 distance, float maxTime)
        {
            var timeAssesment = new Dichotomy<float>(GetHeight, v => v, 0, maxTime).GetSolution(0.01f);
            if (!timeAssesment.HasValue)
            {
                return null;
            }

            var time = timeAssesment.Value.Item1;
            var Vy = (distance.y - m_forceY * time) / (time * time);
            if (Mathf.Abs(Vy) > 1)
            {
                //TODO - try another root?
                return null;
            }

            var tangA = 2 * m_speed * time + m_forceY;
            var angle = Mathf.Atan(tangA) * 180f / Mathf.PI;

            var orientation = Quaternion.AngleAxis(angle, Vector3.right) * Vector3.Project(distance, Vector3.up).normalized;
            return new Calibration
            {
                Time = time,
                Orientation = orientation,
            };

            float GetHeight(float t)
            {
                //TODO - find a way tro optimize following calculations.
                return Mathf.Pow(m_speed, 2) * Mathf.Pow(t, 4)
                    - (Mathf.Pow(distance.z, 2) + Mathf.Pow(distance.x, 2) + m_speed * Mathf.Pow(m_forceY, 2)) * Mathf.Pow(t, 2)
                    + 2 * m_forceY * distance.y * Mathf.Pow(m_speed, 2) * t
                    - Mathf.Pow(m_speed, 2) * Mathf.Pow(distance.y, 2);
            }
        }

        private void Awake()
        {
            m_start = transform.position;
            m_forces = new Vector3(0, m_forceY, 0);
            m_time = 0f;
        }

        private void Update()
        {
            m_time += Time.deltaTime;
            //TODO - implement movement here.

            transform.position = m_start + m_speed * m_time * transform.forward + m_speed * m_speed * m_forces;
        }
    }
}