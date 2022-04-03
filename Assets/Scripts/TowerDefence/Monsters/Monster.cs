using System;
using UnityEngine;

namespace TowerDefence.Monsters
{
    //TODO - turn into an Object for ObjectPool.
    public sealed class Monster : MonoBehaviour, IMonster
	{
		public event Action Died;
		public event Action Released;

		public int m_maxHP = 30;

		[SerializeField]
		private PathAgent _navigation;

		private int m_HP;

		public int HP
		{
			get => m_HP;
			set
			{
				m_HP = value;
				if (m_HP <= 0)
				{
					Died?.Invoke();
					//TODO - play death animation?
					Release();
				}
			}
		}

		public PathAgent Navigation => _navigation;

		IMover IMonster.Mover => _navigation;

		public void Release()
		{
			Released?.Invoke();
			//TODO - do not destroy. Return to object pool instead.
			Destroy(gameObject);
		}

		private void Start()
		{
			HP = m_maxHP;
			_navigation.SetProgress(0);
		}
    }
}