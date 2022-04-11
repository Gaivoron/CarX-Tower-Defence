using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
        public override ICalibration Target(Vector3 distance, float _)
        {
            var A = m_forceY * m_forceY;
            var B = -(2 * m_forceY * distance.y + m_speed * m_speed);
            var C = distance.sqrMagnitude;

            var D = B * B - 4 * A * C;
            if (D < 0)
            {
                Debug.Log($"No solution for distance {distance}");
                return null;
            }

            var d = Mathf.Sqrt(D);
            var times = new List<float>(4);
            var T21 = 0.5f * (-B + d) / A;
            if (T21 > 0)
            {
                times.Add(Mathf.Sqrt(T21));
            }
            var T22 = 0.5f * (-B - d) / A;
            if (T22 > 0)
            {
                times.Add(Mathf.Sqrt(T22));
            }
            if (!times.Any())
            {
                return null;
            }

            var time = times.Min();
            var Vy = (distance.y - m_forceY * time * time) / (time);
            Debug.Log($"{GetType().Name}.{nameof(Target)} got {nameof(Vy)} = {Vy}");
            if (Mathf.Abs(Vy) > m_speed)
            {
                //TODO - try another root?
                return null;
            }

            var orientation = Vector3.ProjectOnPlane(distance, Vector3.up).normalized * Mathf.Sqrt(m_speed * m_speed - Vy * Vy);
            orientation.y = Vy;

            return new Calibration
            {
                Time = time,
                Orientation = orientation,
            };
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