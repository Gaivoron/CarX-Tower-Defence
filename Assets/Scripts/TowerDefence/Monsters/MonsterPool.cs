using UnityEngine;
using Shared.ObjectPool;
using System.Collections.Generic;

namespace TowerDefence.Monsters
{
    public sealed class MonsterPool : Pool<Monster>
    {
		private static MonsterPool m_instance;

		public static IPool<Monster> Instance
		{
            get
			{
				if (m_instance == null)
				{
					m_instance = new MonsterPool();
				}

				return m_instance;
			}
		}

		private MonsterPool()
			: base(CreateMonsters)
        {
        }

		private static IEnumerable<Monster> CreateMonsters()
		{
			var prefab = Resources.Load<Monster>("Minion");
			return new[] { Object.Instantiate(prefab) };
		}
	}
}