using System;
using UnityEngine;

namespace TowerDefence.Monsters
{

    public sealed class MonsterSpawner : MonoBehaviour
	{
		public event Action<IMonster> Spawned;

		[SerializeField]
		private Monster m_monsterPrefab;
		[SerializeField]
		private float m_interval = 3;
		[SerializeField]
		private GameObject m_moveTarget;

		private float m_lastSpawn = -1;

		private void Update()
		{
			if (Time.time > m_lastSpawn + m_interval)
			{
				SpawnMonster();
				m_lastSpawn = Time.time;
			}
		}

		private void SpawnMonster()
		{
			var monster = Instantiate(m_monsterPrefab, transform.position, Quaternion.identity);
			monster.MoveTarget = m_moveTarget;
			Spawned?.Invoke(monster);
		}
	}
}