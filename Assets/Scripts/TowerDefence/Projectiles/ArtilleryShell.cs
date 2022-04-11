using UnityEngine;
using Shared.Mathematics;
using System.Linq;

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
            var times = new Roots(GetHeight, 4, 0, maxTime).GetSolutions(0.0001f);
            if (!times.Any())
            {
                return null;
            }

            var time = times.Select(any => any.Item1).Min();
            var Vy = (distance.y - m_forceY * time * time) / (time);
            if (Mathf.Abs(Vy) > m_speed)
            {
                Debug.LogWarning($"{GetType().Name}.{nameof(Target)} got {nameof(Vy)} = {Vy}");
                //TODO - try another root?
                return null;
            }

            var tangA = 2 * m_forceY * time + Vy;
            var angle = Mathf.Atan(tangA) * 180f / Mathf.PI;
            Debug.Log($"{GetType().Name}.{nameof(Target)} got {nameof(angle)} = {angle}");
            var flatDirection = Vector3.ProjectOnPlane(distance, Vector3.up).normalized;
            var rotationAxis = Vector3.Cross(flatDirection, Vector3.up);
            var orientation = Quaternion.AngleAxis(angle, rotationAxis) * flatDirection;
            Debug.Log($"{GetType().Name}.{nameof(Target)} : {distance} -> {flatDirection} -> {orientation}");
            return new Calibration
            {
                Time = time,
                Orientation = orientation,
            };

            float GetHeight(float t)
            {
                return m_forceY * m_forceY * t * t * t * t
                    - t * t * (2 * m_forceY * distance.y + m_speed * m_speed)
                    + distance.y * distance.y - distance.x * distance.x - -distance.z * distance.z;
                /*
                return Mathf.Pow(m_speed, 2) * Mathf.Pow(t, 4)
                    - (Mathf.Pow(distance.z, 2) + Mathf.Pow(distance.x, 2) + m_speed * Mathf.Pow(m_forceY, 2)) * Mathf.Pow(t, 2)
                    + 2 * m_forceY * distance.y * Mathf.Pow(m_speed, 2) * t
                    - Mathf.Pow(m_speed, 2) * Mathf.Pow(distance.y, 2);
                */
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

            transform.position = GetPosition(m_time);
        }

        private void OnDrawGizmos()
        {
            var color = Gizmos.color;
            Gizmos.color = Color.red;
            DrawLines(m_time, m_time + 1, m_time + 2, m_time + 3, m_time + 5);
            Gizmos.color = color;
        }

        private void DrawLines(params float[] timestamps)
        {
            DrawLines(timestamps.Select(GetPosition).ToArray());
        }

        private void DrawLines(Vector3[] positions)
        {
            var count = positions.Length - 1;
            for (var i = 0; i < count; ++i)
            {
                Gizmos.DrawLine(positions[i], positions[i + 1]);
            }
        }

        private Vector3 GetPosition(float time)
        {
            return m_start + m_speed * time * transform.forward + time * time * m_forces;
        }
    }
}