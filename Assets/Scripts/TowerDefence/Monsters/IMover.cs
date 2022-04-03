using System;
using UnityEngine;

namespace TowerDefence.Monsters
{
    public interface IMover
    {
        event Action FinishReached;

        Vector3 Position { get; }
    }
}