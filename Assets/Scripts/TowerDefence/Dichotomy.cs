using System;
using UnityEngine;

namespace TowerDefence
{
    public sealed class Dichotomy<T>
	{
		private readonly Func<float, T> m_func;
        private readonly Func<T, float> m_selector;

        private readonly (float, T) m_point1;
		private readonly (float, T) m_point2;

        public Dichotomy(Func<float, T> func, Func<T, float> selector, float start, float end)
			: this(func, selector, (start, func(start)), (end, func(end)))
        {
        }

		private Dichotomy(Func<float, T> func, Func<T, float> selector, (float, T) point1, (float, T) point2)
		{
			m_func = func;
			m_selector = selector;

			m_point1 = point1;
			m_point2 = point2;
		}

		//TODO - add function value tolerance as well.
        public (float, T)? GetSolution(float distanceTolerance)
		{
			var x1 = m_point1.Item1;
			var x2 = m_point2.Item1;

			var y1 = m_selector(m_point1.Item2);
			var y2 = m_selector(m_point2.Item2);

			if (Mathf.Sign(y1) * Mathf.Sign(y2) > 0)
			{
				return null;
			}

			var middleX = (x1 + x2) * 0.5f;
			var middleValue = m_func(middleX);
			var middlePoint = (middleX, middleValue);
			if (x2 - x1 <= distanceTolerance)
			{
				return middlePoint;
			}

			var middleY = m_selector(middleValue);
			var iteration = Mathf.Sign(y1) * Mathf.Sign(middleY) <= 0 ? new Dichotomy<T>(m_func, m_selector, m_point1, middlePoint) : new Dichotomy<T>(m_func, m_selector, middlePoint, m_point2);
			return iteration.GetSolution(distanceTolerance);
		}
	}
}