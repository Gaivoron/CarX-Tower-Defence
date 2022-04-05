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

			return new Solution(this, forecastedPosition, timing.Value.Item2);

			float GetShootingTiming(float hitTime)
			{
				var predictedPosition = mover.Position;
				//TODO - take into account actual position of m_shootPoint when cannon will be facing predicted point?
				var flightTime = Vector3.Distance(m_shootPoint.position, predictedPosition) / m_projectilePrefab.Speed;

				var preparationTime = hitTime - flightTime;
				return preparationTime;
			}
		}

		private sealed class Solution : ISolution
		{
			private readonly CannonPlatform m_canon;

			private readonly float m_rotationY;
			private readonly float m_rotationX;

			//TODO - turn predictedPosition into rotation deltas.
			public Solution(CannonPlatform canon, Vector3 predictedPosition, float rotationDuration)
			{
				m_canon = canon;
			}

            async UniTask<bool> ISolution.ExecuteAsync()
            {
				//TODO - implement rotation here.
				await UniTask.WaitUntil(() => false);
				Instantiate(m_canon.m_projectilePrefab, m_canon.m_shootPoint.position, m_canon.m_shootPoint.rotation);
				return true;
			}
        }
	}
}