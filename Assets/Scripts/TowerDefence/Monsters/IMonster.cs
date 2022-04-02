using UnityEngine;
using System;

namespace TowerDefence.Monsters
{
    public interface IMonster
	{
		event Action Died;
		event Action Released;

		int HP { get; set; }

		//TODO - move into a seperate interface that will be responsible for movement exclusively?
		Vector3 Position { get; }
    }
}