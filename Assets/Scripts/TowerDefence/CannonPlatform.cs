using Cysharp.Threading.Tasks;
using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence
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
			var timing = new Dichotomy(GetShootingTiming, 0, maxTime).GetSolution(1f / 24);
			if (!timing.HasValue)
			{
				return null;
			}

			var forecastedPosition = mover.PredictPosition(timing.Value.Item1);
			if (!IsWithinReach(forecastedPosition))
			{
				return null;
			}

			return new Solution(this, forecastedPosition);

			float GetShootingTiming(float hitTime)
			{
				var predictedPosition = mover.Position;
				//TODO - take into account actual position of m_shootPoint when cannon will be facing predicted point?
				var flightTime = Vector3.Distance(m_shootPoint.position, predictedPosition) / m_projectilePrefab.Speed;

				var preparationTime = hitTime - flightTime;
				return preparationTime;
			}
		}

        protected override void DrawGizmos()
        {
            base.DrawGizmos();
			Gizmos.DrawRay(m_shootPoint.position, m_shootPoint.forward * 100);
        }

        private sealed class Solution : ISolution
		{
			private readonly CannonPlatform m_canon;

			private readonly float m_rotationY;
			//private readonly float m_rotationX;

			private readonly Vector3 m_predictedPosition;

			//TODO - turn predictedPosition into rotation deltas.
			//TODO - pass float rotationDuration?
			public Solution(CannonPlatform canon, Vector3 predictedPosition)
			{
				m_canon = canon;
				var direction = predictedPosition - m_canon.transform.position;
				m_rotationY = Vector3.SignedAngle(m_canon.m_yRotor.forward, direction, Vector3.up);
				//m_rotationX = -Vector3.SignedAngle(m_canon.m_xRotor.forward, direction, m_canon.m_xRotor.right);
				//Debug.Log(m_rotationX);
				m_predictedPosition = predictedPosition;
			}

            async UniTask<bool> ISolution.ExecuteAsync()
            {
				//TODO - implement rotation here.
				//var rotationY = m_canon.m_yRotor.localRotation.eulerAngles;
				//m_canon.m_xRotor.RotateAround(m_canon.m_xRotor.position, m_canon.m_xRotor.right, m_rotationX);
				m_canon.m_yRotor.RotateAround(m_canon.m_yRotor.position, Vector3.up, m_rotationY);
				m_canon.m_xRotor.LookAt(m_predictedPosition);
				Instantiate(m_canon.m_projectilePrefab, m_canon.m_shootPoint.position, m_canon.m_shootPoint.rotation);
				return true;
			}
        }
	}
}