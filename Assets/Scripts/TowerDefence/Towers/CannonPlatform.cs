using Shared.Mathematics;
using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed partial class CannonPlatform : WeaponPlatform
	{
		private const float TimeThreshold = 1f / 24;

		[SerializeField]
		private CannonBall m_projectilePrefab;
		[SerializeField]
		private Transform m_shootPoint;

		[Space]
		[Header("Rotation")]
		[SerializeField]
		private Transform m_yRotor;
		[SerializeField]
		private float m_yRotationSpeed = 0.5f;
		[SerializeField]
		private Transform m_xRotor;
		[SerializeField]
		private float m_xRotationSpeed = 180;

		private float m_currentXRotation = 0f;
		private float m_currentYRotation = 0f;
        private ITargetData m_solution;

        private Vector3 CurrentRotations => new Vector3(m_currentXRotation, m_currentYRotation);

		protected override ISolution AcquireSolution(IMonster target)
		{
			if (m_projectilePrefab == null)
			{
				return null;
			}

			var mover = target.Mover;
			var maxTime = mover.EstimatedTime;
			var targeting = new Dichotomy<ITargetData>(GetTargetingData, Mesure, 0, maxTime).GetSolution(TimeThreshold);
			if (!targeting.HasValue)
			{
				return null;
			}

			if (!IsWithinReach(targeting.Value.Item2.Point))
			{
				return null;
			}

			m_solution = targeting.Value.Item2;
			return new CannonFiringSolution(this, targeting.Value.Item2);

			ITargetData GetTargetingData(float hitTime)
			{
				var predictedPosition = mover.Position;
				//TODO - take into account actual position of m_shootPoint when cannon will be facing predicted point?
				var direction = predictedPosition - m_shootPoint.position;
				var flightTime = direction.magnitude / m_projectilePrefab.Speed;

				var currentOrientation = m_shootPoint.forward;
				var yDelta = GetRotation(Vector3.up);
				var xDelta = GetRotation(Vector3.right);

				return new TargetData
				{
					Point = predictedPosition,
					HitTime = hitTime,
					FlightTime = flightTime,
					Angles = CurrentRotations + new Vector3(xDelta, yDelta),
					RotationTime = Mathf.Max(Mathf.Abs(yDelta / m_yRotationSpeed), Mathf.Abs(xDelta / m_xRotationSpeed))
				};

				float GetRotation(Vector3 axis) => Vector3.SignedAngle(Vector3.ProjectOnPlane(currentOrientation, axis), Vector3.ProjectOnPlane(direction, axis), axis);
			}

			float Mesure(ITargetData data) => data.HitTime - data.FlightTime - data.RotationTime;
		}

		protected override void DrawGizmos()
		{
			base.DrawGizmos();
			Gizmos.DrawRay(m_shootPoint.position, m_shootPoint.forward * m_range);

			if (m_solution != null)
			{
				var color = Gizmos.color;
				Gizmos.color = Color.green;
				Gizmos.DrawRay(m_shootPoint.position, m_solution.Point - m_shootPoint.position);
				Gizmos.color = color;
			}
		}
	}
}