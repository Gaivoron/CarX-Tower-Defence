using System;
using UnityEngine;
using Shared.ObjectPool;

namespace TowerDefence.Monsters
{
    public sealed class Monster : MonoBehaviour, ITarget, IObject<MonsterType>
	{
		public event Action Died;
		public event Action Released;

		public int m_maxHP = 30;

		[SerializeField]
		[UnityEngine.Serialization.FormerlySerializedAs("_navigation")]
		private PathAgent m_navigation;

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

		public PathAgent Navigation => m_navigation;

		IMover ITarget.Mover => m_navigation;

        public MonsterType Key
		{
			get;
			set;
		}

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