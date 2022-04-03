using System;
using UnityEngine;

namespace TowerDefence.Monsters
{
    public sealed class PathAgent : MonoBehaviour, IMover
	{
		public event Action FinishReached;

		[SerializeField]
		private float m_speed = 0.1f;

        private IPath m_path;
		private float m_progress;

		Vector3 IMover.Position => transform.position;

		public void SetPath(IPath path)
		{
			m_path = path;
		}

        public void SetProgress(float progress)
        {
			transform.position = m_path.GetPosition(progress);
			m_progress = progress;
		}

        private void Update()
		{
			if (m_path == null)
			{
				return;
			}

			MoveForward();
			if (m_progress >= m_path.Length)
			{
				FinishReached?.Invoke();
			}
		}

		private void MoveForward()
		{
			var distance = m_speed * Time.deltaTime;
			SetProgress(m_progress + distance);
		}
	}
}