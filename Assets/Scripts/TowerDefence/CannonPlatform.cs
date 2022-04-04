using System;
using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence
{
	public sealed class CannonPlatform : WeaponPlatform
	{
		[SerializeField]
		private CannonBall m_projectilePrefab;
		public Transform m_shootPoint;

		[Space]
		[Header("Rotation")]
		[SerializeField]
		private Transform m_yRotor;
		[SerializeField]
		private Transform m_xRotor;

		//TODO - should accept ISolution instead of IMonster.
		protected override bool Shoot(IMonster target)
        {
			if (m_projectilePrefab == null)
			{
				return false;
			}

			var mover = target.Mover;
			var maxTime = mover.EstimatedTime;
			var timing = new Dichotomy(GetShootingTiming, 0, maxTime).GetSolution();
			if (!timing.HasValue || !IsWithinReach(mover.PredictPosition(timing.Value.Item1)))
			{
				return false;
			}

			Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
			return true;

			float GetShootingTiming(float hitTime)
			{
				var predictedPosition = mover.Position;
				//TODO - take into account actual position of m_shootPoint when cannon will be facing predicted point?
				var flightTime = Vector3.Distance(m_shootPoint.position, predictedPosition) / m_projectilePrefab.m_speed;

				var preparationTime = hitTime - flightTime;
				/*
				if (preparationTime < 0)
				{
					preparationTime = float.MaxValue;
				}
				*/
				return preparationTime;
			}
		}
	}

	public sealed class Dichotomy
	{
		//TODO - turn into another constructor parameter?
		private const float Tolerance = 0.0001f;

		private readonly Func<float, float> m_func;
        private readonly (float, float) m_point1;
		private readonly (float, float) m_point2;
		//private readonly float m_start;
        //private readonly float m_end;

        public Dichotomy(Func<float, float> func, float start, float end)
			: this(func, (start, func(start)), (end, func(end)))
        {
        }

		private Dichotomy(Func<float, float> func, (float, float) point1, (float, float) point2)
		{
			m_func = func;

			m_point1 = point1;
			m_point2 = point2;
		}

		//TODO - return nullable value?
        public (float, float)? GetSolution()
		{
			var x1 = m_point1.Item1;
			var x2 = m_point2.Item1;

			var y1 = m_point1.Item2;
			var y2 = m_point1.Item2;

			if (Mathf.Sign(y1) * Mathf.Sign(y2) > 0)
			{
				return null;
			}

			var middleX = (x1 + x2) * 0.5f;
			var middleY = m_func(middleX);
			var middlePoint = (middleX, middleY);
			if (x2 - x1 <= Tolerance)
			{
				return middlePoint;
			}

			Debug.Log($"{Mathf.Sign(y1) * Mathf.Sign(middleY)} vs {Mathf.Sign(middleY) * Mathf.Sign(y2)}");

			var iteration = Mathf.Sign(y1) * Mathf.Sign(middleY) <= 0 ? new Dichotomy(m_func, m_point1, middlePoint) : new Dichotomy(m_func, middlePoint, m_point2);
			return iteration.GetSolution();
		}
	}

	public interface ISolution
	{
		/*
		float Y { get; }
		float X { get; }
		*/
	}
}