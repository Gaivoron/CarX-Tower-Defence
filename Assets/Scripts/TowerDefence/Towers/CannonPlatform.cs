using Cysharp.Threading.Tasks;
using Shared.Mathematics;
using System.Threading;
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

			return new Solution(this, targeting.Value.Item2);

			ITargetData GetTargetingData(float hitTime)
			{
				var predictedPosition = mover.Position;
				//TODO - take into account actual position of m_shootPoint when cannon will be facing predicted point?
				var direction = predictedPosition - m_shootPoint.position;
				var flightTime = direction.magnitude / m_projectilePrefab.Speed;

				var currentOrientation = m_shootPoint.forward;
				var yDelta = GetRotation(Vector3.up);
				var xDelta = GetRotation(Vector3.left);

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
		}

		private sealed class Solution : ISolution
		{
			private const float TimeThreshold = 1f / 24;
			private const float AngularThreshold = 0.001f;

			private readonly CannonPlatform m_canon;
			private readonly ITargetData m_data;
			//private readonly float m_rotationY;
			//private readonly float m_rotationX;

			//TODO - turn predictedPosition into rotation deltas.
			public Solution(CannonPlatform canon, ITargetData data)
			{
				m_canon = canon;
				m_data = data;
			}

			async UniTask<bool> ISolution.ExecuteAsync(CancellationToken cancellation)
			{
				await Rotate(cancellation);

				m_canon.m_xRotor.LookAt(m_data.Point);
				Instantiate(m_canon.m_projectilePrefab, m_canon.m_shootPoint.position, m_canon.m_shootPoint.rotation);
				return true;
			}

			private async UniTask Rotate(CancellationToken cancellation)
			{
				await UniTask.WhenAll(RotateAroundY(cancellation));
				//TODO - implement rotation here.
			}

			private async UniTask RotateAroundY(CancellationToken cancellation)
			{
				var time = 0f;
				var targetAngle = m_data.Angles.y;
				var sign = Mathf.Sign(GetTail());
				while (m_data.RotationTime > time + TimeThreshold && Mathf.Abs(GetTail()) > AngularThreshold)
				{
					await UniTask.Yield(cancellation);
					time += Time.deltaTime;
					var delta = Time.deltaTime * m_canon.m_yRotationSpeed;
					var tail = Mathf.Abs(GetTail());
					if (delta > tail)
					{
						delta = tail;
					}
					if (delta > 0 && sign < 0)
					{
						delta = -delta;
					}

					m_canon.m_currentYRotation += delta;
					m_canon.m_yRotor.Rotate(Vector3.up, delta);
				}

				float GetTail() => targetAngle - m_canon.m_currentYRotation;
			}
		}
	}
}