using System;
using UnityEngine;
using Shared.ObjectPool;

namespace TowerDefence.Monsters
{
    public sealed class Monster : MonoBehaviour, IMonster, IObject
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
				Debug.Log($"{name}.{GetType().Name}.{nameof(HP)} = {value}");
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
			Navigation.SetPath(null);
			gameObject.SetActive(false);
		}

        void IObject.Init()
        {
			gameObject.SetActive(true);
			HP = m_maxHP;
		}
    }
}