using UnityEngine;
using System;

namespace TowerDefence.Monsters
{
    public interface ITarget
	{
		event Action Died;
		event Action Released;

		int HP { get; set; }
		IMover Mover { get; }

        void Release();
    }
}