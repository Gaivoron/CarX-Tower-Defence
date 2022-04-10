using Shared.Mathematics;
using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed partial class CannonPlatform : WeaponPlatform
	{
		private const float TimeThreshold = 1f / 100;

		[SerializeField]
		private CannonBall m_projectilePrefab;
		[SerializeField]
		private Transform m_shootPoint;

		[Space]
		[Header("Rotation")]
		[SerializeField]
		private Rotor m_yRotor;
		[SerializeField]
		private Rotor m_xRotor;

        //private ITargetData m_solution;

        private Vector3 CurrentRotations => new Vector3(m_xRotor.Angle, m_yRotor.Angle);

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

			//m_solution = targeting.Value.Item2;
			return new CannonFiringSolution(this, targeting.Value.Item2);

			ITargetData GetTargetingData(float hitTime)
			{
				var predictedPosition = mover.PredictPosition(hitTime);

				var startingPosition = m_shootPoint.position;
				var direction = predictedPosition - startingPosition;
				var flightTime = direction.magnitude / m_projectilePrefab.Speed;
				var orientationY = m_yRotor.Forward;
				var directionY = Vector3.ProjectOnPlane(direction, m_yRotor.Axis);
				var yDelta = Vector3.SignedAngle(orientationY, directionY, m_yRotor.Axis);
				var orientationX = Quaternion.AngleAxis(yDelta, m_yRotor.Axis) * m_xRotor.Forward;
				var xDelta = Vector3.SignedAngle(orientationX, direction, m_xRotor.Axis);

				return new TargetData
				{
					Point = predictedPosition,
					HitTime = hitTime,
					FlightTime = flightTime,
					Angles = CurrentRotations + new Vector3(xDelta, yDelta),
					RotationTime = Mathf.Max(Mathf.Abs(xDelta / m_xRotor.Speed), Mathf.Abs(yDelta / m_yRotor.Speed))
				};
            }

            float Mesure(ITargetData data) => data.HitTime - data.FlightTime - data.RotationTime;
		}

		protected override void DrawGizmos()
		{
			base.DrawGizmos();
			Gizmos.DrawRay(m_shootPoint.position, m_shootPoint.forward * m_range);
			/*
			if (m_solution != null)
			{
				var color = Gizmos.color;
				Gizmos.color = Color.green;
				Gizmos.DrawRay(m_shootPoint.position, m_solution.Point - m_shootPoint.position);
				Gizmos.color = color;
			}
			*/
		}
	}
}