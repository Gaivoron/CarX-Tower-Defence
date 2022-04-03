using UnityEngine;
using System;

namespace TowerDefence.Monsters
{
    public interface IMonster
	{
		event Action Died;
		event Action Released;

		int HP { get; set; }
		IMover Mover { get; }

        void Release();
    }
}