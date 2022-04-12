using System;
using UnityEngine;

namespace TowerDefence.Monsters
{
    public sealed class MonsterSpawner : MonoBehaviour
	{
		public event Action<ITarget> Spawned;

		[SerializeField]
		private float m_interval = 3;
		[SerializeField]
		private LinearPath m_path;

		private float m_lastSpawn = 0;

		private void Update()
		{
			if (Time.time > m_lastSpawn + m_interval)
			{
				SpawnMonster();
				m_lastSpawn = Time.time;
			}
		}

		//TODO - spawn all kinds of monster-variations.
		private void SpawnMonster()
		{
			var monster = MonsterPool.Instance.Get();
			monster.Navigation.SetPath(m_path);
			monster.Navigation.SetProgress(0);
			Spawned?.Invoke(monster);
		}
	}
}