using UnityEngine;

namespace TowerDefence.Towers
{
    //TODO - that class should be responsible for upgrading and\or selling tower.
    public class Tower : MonoBehaviour
	{
		[SerializeField]
		private WeaponPlatform m_platform;

        private IGameplayData m_data;

        public void Initialize(IGameplayData data)
		{
			m_data = data;

			if (m_platform != null)
			{
				m_platform.Initialize(m_data);
			}
		}
	}
}