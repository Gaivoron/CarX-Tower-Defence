using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace TowerDefence.Monsters
{
    public sealed class MonsterSpawner : MonoBehaviour
	{
		public event Action<ITarget> Spawned;

		[SerializeField]
		private LinearPath m_path;

        private IGameplayData m_data;
        private CancellationTokenSource m_cancellation;

        public void Initialize(IGameplayData data)
		{
			m_data = data;
			m_data.Defeated += OnGameOver;
		}

        public void StartWave(IWave wave)
		{
			m_cancellation = new CancellationTokenSource();
			StartWaveAsync(wave, m_cancellation.Token).Forget();
		}

		private async UniTask StartWaveAsync(IWave wave, CancellationToken token)
		{
			foreach (var instruction in wave.Instructions)
			{
				var delay = instruction.Delay;
				if (delay > 0)
				{
					await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
				}

				SpawnMonster(instruction.Archetype);
			}
		}

		//TODO - spawn all kinds of monster-variations.
		private void SpawnMonster(MonsterType archetype)
		{
			var monster = MonsterPool.Instance.Get(archetype);
			monster.Navigation.SetPath(m_path);
			monster.Navigation.SetProgress(0);
			Spawned?.Invoke(monster);
		}

		private void OnGameOver()
		{
			m_data.Defeated -= OnGameOver;
			m_cancellation?.Cancel();
		}
	}
}