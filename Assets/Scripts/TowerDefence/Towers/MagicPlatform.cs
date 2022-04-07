using Cysharp.Threading.Tasks;
using System.Threading;
using TowerDefence.Monsters;
using TowerDefence.Projectiles;
using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed class MagicPlatform : WeaponPlatform
	{
		[SerializeField]
		private GuidedProjectile m_projectilePrefab;

		[SerializeField]
		private Transform m_spawnPoint;

        protected override ISolution AcquireSolution(IMonster target)
        {
			if (m_projectilePrefab == null)
			{
				return null;
			}

			return new Solution(this, target);
		}

		private sealed class Solution : ISolution
		{
            private readonly MagicPlatform m_platform;
            private readonly IMonster m_target;

            public Solution(MagicPlatform platform, IMonster target)
			{
				m_platform = platform;
				m_target = target;
			}

            UniTask<bool> ISolution.ExecuteAsync(CancellationToken cancellation)
            {
				var projectile = Instantiate(m_platform.m_projectilePrefab, m_platform.m_spawnPoint.position, Quaternion.identity);
				projectile.SetTarget(m_target);
				return new UniTask<bool>(true);
            }
        }
    }
}