using System;
using UnityEngine;

namespace TowerDefence
{
    public sealed class Dichotomy
	{
		private readonly Func<float, float> m_func;
        private readonly (float, float) m_point1;
		private readonly (float, float) m_point2;

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

		//TODO - add function value tolerance as well.
        public (float, float)? GetSolution(float distanceTolerance)
		{
			var x1 = m_point1.Item1;
			var x2 = m_point2.Item1;

			var y1 = m_point1.Item2;
			var y2 = m_point2.Item2;

			if (Mathf.Sign(y1) * Mathf.Sign(y2) > 0)
			{
				return null;
			}

			var middleX = (x1 + x2) * 0.5f;
			var middleY = m_func(middleX);
			var middlePoint = (middleX, middleY);
			if (x2 - x1 <= distanceTolerance)
			{
				return middlePoint;
			}

			Debug.Log($"{Mathf.Sign(y1) * Mathf.Sign(middleY)} vs {Mathf.Sign(middleY) * Mathf.Sign(y2)}");

			var iteration = Mathf.Sign(y1) * Mathf.Sign(middleY) <= 0 ? new Dichotomy(m_func, m_point1, middlePoint) : new Dichotomy(m_func, middlePoint, m_point2);
			return iteration.GetSolution(distanceTolerance);
		}
	}
}