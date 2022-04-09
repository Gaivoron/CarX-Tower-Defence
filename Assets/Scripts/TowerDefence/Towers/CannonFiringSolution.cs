using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace TowerDefence.Towers
{
    public sealed partial class CannonPlatform
    {
        private sealed class CannonFiringSolution : ISolution
		{
			private const float TimeThreshold = 1f / 60;
			private const float AngularThreshold = 0.001f;

			private readonly CannonPlatform m_canon;
			private readonly ITargetData m_data;

			public CannonFiringSolution(CannonPlatform canon, ITargetData data)
			{
				Debug.Log($"{GetType().Name}({data})");
				m_canon = canon;
				m_data = data;
			}

			async UniTask<bool> ISolution.ExecuteAsync(CancellationToken cancellation)
			{
				await UniTask.WhenAll(RotateAroundYAsync(cancellation), RotateAroundXAsync(cancellation));

				//m_canon.m_xRotor.LookAt(m_data.Point);
				Instantiate(m_canon.m_projectilePrefab, m_canon.m_shootPoint.position, m_canon.m_shootPoint.rotation);
				return true;
			}

			private async UniTask RotateAroundXAsync(CancellationToken cancellation)
			{
				await RotateAsync(() => m_data.Angles.x - m_canon.m_currentXRotation, Rotate, cancellation);

				void Rotate(float delta)
				{
					m_canon.m_currentXRotation += delta;
					m_canon.m_xRotor.Rotate(Vector3.right, delta);
				}
			}

			private async UniTask RotateAroundYAsync(CancellationToken cancellation)
			{
				await RotateAsync(() => m_data.Angles.y - m_canon.m_currentYRotation, Rotate, cancellation);

				void Rotate(float delta)
				{
					m_canon.m_currentYRotation += delta;
					m_canon.m_yRotor.Rotate(Vector3.up, delta);
				}
			}

			private async UniTask RotateAsync(Func<float> getTail, Action<float> rotate, CancellationToken cancellation)
			{
				var time = 0f;
				var sign = Mathf.Sign(getTail());
				while (m_data.RotationTime > time + TimeThreshold && getTail() * sign > AngularThreshold)
				{
					await UniTask.Yield(cancellation);
					time += Time.deltaTime;
					var delta = Time.deltaTime * m_canon.m_yRotationSpeed;
					var tail = Mathf.Abs(getTail());
					if (delta > tail)
					{
						delta = tail;
					}
					if (delta > 0 && sign < 0)
					{
						delta = -delta;
					}

					rotate(delta);
				}
			}
		}
	}
}