using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed partial class CannonPlatform
    {
        private sealed class CannonFiringSolution : ISolution
		{
			private const float TimeThreshold = 0.001f;
			private const float AngularThreshold = 0.001f;

			private readonly CannonPlatform m_canon;
			private readonly ITargetData m_data;

			public CannonFiringSolution(CannonPlatform canon, ITargetData data)
			{
				m_canon = canon;
				m_data = data;
			}

			async UniTask<bool> ISolution.ExecuteAsync(CancellationToken cancellation)
			{
				await UniTask.WhenAll(RotateAroundYAsync(cancellation), RotateAroundXAsync(cancellation));

				Instantiate(m_canon.m_projectilePrefab, m_canon.m_shootPoint.position, m_canon.m_shootPoint.rotation);
				return true;
			}

			private async UniTask RotateAroundXAsync(CancellationToken cancellation)
			{
				await RotateAsync(m_canon.m_xRotor, m_data.Angles.x, cancellation);
			}

			private async UniTask RotateAroundYAsync(CancellationToken cancellation)
			{
				await RotateAsync(m_canon.m_yRotor, m_data.Angles.y, cancellation);
			}

			private async UniTask RotateAsync(Rotor rotor, float target, CancellationToken cancellation)
			{
				var time = 0f;
				var sign = Mathf.Sign(GetTail());
				while (m_data.RotationTime > time + TimeThreshold && GetTail() * sign > AngularThreshold)
				{
					await UniTask.Yield(cancellation);
					time += Time.deltaTime;
					var delta = Time.deltaTime * rotor.Speed;
					var tail = Mathf.Abs(GetTail());
					if (delta > tail)
					{
						delta = tail;
					}
					if (delta > 0 && sign < 0)
					{
						delta = -delta;
					}

					rotor.Angle += delta;
				}
				rotor.Angle = target;

				float GetTail() => target - rotor.Angle;
			}
		}
	}
}