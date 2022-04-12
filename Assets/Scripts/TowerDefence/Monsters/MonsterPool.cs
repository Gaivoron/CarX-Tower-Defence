using UnityEngine;
using Shared.ObjectPool;
using System.Collections.Generic;

namespace TowerDefence.Monsters
{
    public sealed class MonsterPool : Pool<MonsterType, Monster>
    {
		private static MonsterPool m_instance;

		public static IPool<MonsterType, Monster> Instance
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

		private static IEnumerable<Monster> CreateMonsters(MonsterType key)
		{
			var prefab = Resources.Load<Monster>(key.ToString());
			var monster = Object.Instantiate(prefab);
			monster.Key = key;

			return new[] { monster };
		}
	}
}