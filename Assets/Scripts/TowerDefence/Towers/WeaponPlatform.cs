using UnityEngine;
using System.Linq;
using TowerDefence.Monsters;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace TowerDefence.Towers
{
    public abstract class WeaponPlatform : MonoBehaviour, IPlatform, IRechargeable
	{
		public event Action<float, float> Recharged;

		[UnityEngine.Serialization.FormerlySerializedAs("m_shootInterval")]
		public float m_rechargeDuration = 0.5f;
		public float m_range = 4f;

		private float m_rechargeProgress = 0f;
		private IGameplayData m_data;

        private CancellationTokenSource m_cancellationTokenSource;

        private IEnumerable<ITarget> Monsters => m_data.MonsterRoster.Monsters;

		public void Initialize(IGameplayData data)
		{
			m_data = data;
			m_data.Defeated += OnGameOver;

			m_cancellationTokenSource = new CancellationTokenSource();
			UpdateCycleAsync(m_cancellationTokenSource.Token).Forget();
		}

        protected abstract ISolution AcquireSolution(ITarget target);

		protected bool IsWithinReach(Vector3 position)
		{
			var distance = transform.position - position;
			return distance.sqrMagnitude <= m_range * m_range;
		}

		private async UniTask RechargeAsync(CancellationToken cancellation)
		{
			while (m_rechargeProgress < m_rechargeDuration)
			{
				await UniTask.Yield(cancellation);
				m_rechargeProgress += Time.deltaTime;
				Recharged?.Invoke(m_rechargeProgress, m_rechargeDuration);
			}
		}

		private async UniTask UpdateCycleAsync(CancellationToken cancellation)
		{
			while (true)
			{
				await UpdateAsync(cancellation);
			}
		}

		private async UniTask UpdateAsync(CancellationToken cancellation)
		{
			//recharging shot
			await RechargeAsync(cancellation);

			//finding target
			var solution = await AcquireSolutionAsync(cancellation);

			var hasFired = await solution.ExecuteAsync(cancellation);

			if (hasFired)
			{
				OnShot();
			}
		}

		//TODO - make protected?
		private void OnShot()
		{
			//TODO - turn into a property?
			m_rechargeProgress = 0f;
			Recharged?.Invoke(m_rechargeProgress, m_rechargeDuration);
		}

		private async UniTask<ISolution> AcquireSolutionAsync(CancellationToken cancellation)
		{
			ISolution solution;

			while (true)
			{
				solution = AcquireSolution();
				if (solution != null)
				{
					break;
				}

				await UniTask.Delay(10);
			}

			return solution;
		}

		private ISolution AcquireSolution()
		{
			foreach (var target in Monsters.Where(IsValidTarget))
			{
				var solution = AcquireSolution(target);
				if (solution != null)
				{
					return solution;
				}
			}

			return null;
		}

		private bool IsValidTarget(ITarget target) => IsWithinReach(target.Mover.Position);

		private void OnDrawGizmos()
		{
			var originalColor = Gizmos.color;
			Gizmos.color = Color.yellow;
			DrawGizmos();
			Gizmos.color = originalColor;
		}

		private void OnGameOver()
		{
			m_data.Defeated -= OnGameOver;

			if (m_cancellationTokenSource != null)
			{
				m_cancellationTokenSource.Cancel();
				m_cancellationTokenSource = null;
			}
		}

		protected virtual void DrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, m_range);
		}
	}
}