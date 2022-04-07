using Cysharp.Threading.Tasks;
using Shared.Mathematics;
using System.Threading;
using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence.Towers
{
	public sealed class CannonPlatform : WeaponPlatform
	{
		[SerializeField]
		private CannonBall m_projectilePrefab;
		[SerializeField]
		private Transform m_shootPoint;

		[Space]
		[Header("Rotation")]
		[SerializeField]
		private Transform m_yRotor;
		[SerializeField]
		private Transform m_xRotor;

		protected override ISolution AcquireSolution(IMonster target)
		{
			if (m_projectilePrefab == null)
			{
				return null;
			}

			var mover = target.Mover;
			var maxTime = mover.EstimatedTime;
			var targeting = new Dichotomy<TargetData>(GetTargeyingData, data => data.Time - data.Flight, 0, maxTime).GetSolution(1f / 24);
			if (!targeting.HasValue)
			{
				return null;
			}

			var forecastedPosition = targeting.Value.Item2.Point;
			if (!IsWithinReach(forecastedPosition))
			{
				return null;
			}

			return new Solution(this, targeting.Value.Item2);

			TargetData GetTargeyingData(float hitTime)
			{
				var predictedPosition = mover.Position;
				//TODO - take into account actual position of m_shootPoint when cannon will be facing predicted point?
				var flightTime = Vector3.Distance(m_shootPoint.position, predictedPosition) / m_projectilePrefab.Speed;

				var preparationTime = hitTime - flightTime;
				return new TargetData(predictedPosition, hitTime, flightTime);
			}
		}

		protected override void DrawGizmos()
		{
			base.DrawGizmos();
			Gizmos.DrawRay(m_shootPoint.position, m_shootPoint.forward * 100);
		}

		private struct TargetData
		{
			public Vector3 Point { get; }
			public float Time { get; }
			public float Flight { get; }

			public TargetData(Vector3 point, float time, float flight)
			{
				Point = point;
				Time = time;
				Flight = flight;
			}
		}


		private sealed class Solution : ISolution
		{
			private readonly CannonPlatform m_canon;
			private readonly TargetData m_data;
			private readonly float m_rotationY;
			private readonly float m_rotationX;

			//TODO - turn predictedPosition into rotation deltas.
			public Solution(CannonPlatform canon, TargetData data)
			{
				m_canon = canon;
				m_data = data;
				var direction = data.Point - m_canon.transform.position;
				m_rotationY = Vector3.SignedAngle(m_canon.m_yRotor.forward, direction, Vector3.up);
				//m_rotationX = -Vector3.SignedAngle(m_canon.m_xRotor.forward, direction, m_canon.m_xRotor.right);
				//Debug.Log(m_rotationX);
			}

			async UniTask<bool> ISolution.ExecuteAsync(CancellationToken cancellation)
			{
				//TODO - implement rotation here.
				//var rotationY = m_canon.m_yRotor.localRotation.eulerAngles;
				//m_canon.m_xRotor.RotateAround(m_canon.m_xRotor.position, m_canon.m_xRotor.right, m_rotationX);
				m_canon.m_yRotor.RotateAround(m_canon.m_yRotor.position, Vector3.up, m_rotationY);
				m_canon.m_xRotor.LookAt(m_data.Point);
				Instantiate(m_canon.m_projectilePrefab, m_canon.m_shootPoint.position, m_canon.m_shootPoint.rotation);
				return true;
			}
		}
	}
}