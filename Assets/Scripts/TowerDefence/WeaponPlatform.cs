using UnityEngine;
using System.Linq;
using TowerDefence.Monsters;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace TowerDefence
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

        protected abstract bool Shoot(IMonster target);

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
			Debug.Log($"{name}.{GetType().Name}.{nameof(UpdateAsync)}");
			//recharging shot
			await RechargeAsync();

			//finding target
			var target = await AcquireTargetAsync();
			//await UniTask.SwitchToMainThread();

			Debug.Log(nameof(Shoot));
			if (Shoot(target))
			{
				OnShot();
			}
		}

		//TODO - make protected?
		private void OnShot()
		{
			m_rechargeProgress = 0f;
		}

		private async UniTask<IMonster> AcquireTargetAsync()
		{
			Debug.Log($"{name}.{GetType().Name}.{nameof(AcquireTargetAsync)}");
			IMonster target;

            while (true)
			{
				target = AcquireTarget();
				if (target != null)
				{
					break;
				}

				await UniTask.Yield();
			}

			return target;
		}

		private IMonster AcquireTarget()
		{
			return Monsters.FirstOrDefault(IsValidTarget);
		}

		private bool IsValidTarget(IMonster target) => IsWithinReach(target.Mover.Position);

		protected bool IsWithinReach(Vector3 position)
		{
			var distance = transform.position - position;
			return distance.sqrMagnitude <= m_range * m_range;
		}

		private void OnDrawGizmosSelected()
        {
			var originalColor = Gizmos.color;
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, m_range);
			Gizmos.color = originalColor;
        }
    }
}