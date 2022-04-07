using UnityEngine;
using System.Linq;
using TowerDefence.Monsters;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace TowerDefence.Towers
{
	public abstract class WeaponPlatform : MonoBehaviour, IPlatform
	{
		[UnityEngine.Serialization.FormerlySerializedAs("m_shootInterval")]
		public float m_rechargeDuration = 0.5f;
		public float m_range = 4f;

		private float m_rechargeProgress = 0f;
		private IGameplayData m_data;

		private IEnumerable<IMonster> Monsters => m_data.MonsterRoster.Monsters;

		public void Initialize(IGameplayData data)
		{
			m_data = data;
			UpdateCycleAsync().Forget();
		}

		protected abstract ISolution AcquireSolution(IMonster target);

		protected bool IsWithinReach(Vector3 position)
		{
			var distance = transform.position - position;
			return distance.sqrMagnitude <= m_range * m_range;
		}

		private async UniTask RechargeAsync()
		{
			Debug.Log($"{name}.{GetType().Name}.{nameof(RechargeAsync)}");
			while (m_rechargeProgress < m_rechargeDuration)
			{
				await UniTask.Yield();
				m_rechargeProgress += Time.deltaTime;
			}
		}

		private async UniTask UpdateCycleAsync()
		{
			while (true)
			{
				await UpdateAsync();
			}
		}

		private async UniTask UpdateAsync()
		{
			//Debug.Log($"{name}.{GetType().Name}.{nameof(UpdateAsync)}");
			//recharging shot
			await RechargeAsync();

			//finding target
			var solution = await AcquireSolutionAsync();
			//await UniTask.SwitchToMainThread();

			var hasFired = await solution.ExecuteAsync();

			if (hasFired)
			{
				OnShot();
			}
		}

		//TODO - make protected?
		private void OnShot()
		{
			m_rechargeProgress = 0f;
		}

		private async UniTask<ISolution> AcquireSolutionAsync()
		{
			//Debug.Log($"{name}.{GetType().Name}.{nameof(AcquireSolutionAsync)}");
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

		private bool IsValidTarget(IMonster target) => IsWithinReach(target.Mover.Position);

		private void OnDrawGizmosSelected()
		{
			var originalColor = Gizmos.color;
			Gizmos.color = Color.yellow;
			DrawGizmos();
			Gizmos.color = originalColor;
		}

		protected virtual void DrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, m_range);
		}
	}
}