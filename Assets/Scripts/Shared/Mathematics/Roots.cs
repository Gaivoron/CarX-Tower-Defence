using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Mathematics
{
    public sealed class Roots
	{
		private readonly Func<float, float> m_func;
		private readonly int m_maxRoots;
		private readonly float m_start;
		private readonly float m_end;

		public Roots(Func<float, float> func, int maxRoots, float start, float end)
		{
			m_func = func;
			m_maxRoots = maxRoots;
			m_start = start;
			m_end = end;
		}

		public IEnumerable<(float, float)> GetSolutions(float distanceTolerance)
		{
			var roots = new List<(float, float)>(m_maxRoots);
			for (var i = 0; i < m_maxRoots; ++i)
			{
				var root = new Dichotomy(ModifiedFunc, m_start, m_end).GetSolution(distanceTolerance);
				if (!root.HasValue)
				{
					Debug.Log($"No more roots. Only {roots.Count}");
					break;
				}

				var coord1 = root.Value.Item1;
				roots.Add((coord1, m_func(coord1)));
			}

			Debug.Log($"Total roots count : {roots.Count}");
			return roots;

			float ModifiedFunc(float x)
			{
				var result = m_func(x);
				foreach (var root in roots)
				{
					result /= x - root.Item1;
				}

				return result;
			}
		}
	}
}