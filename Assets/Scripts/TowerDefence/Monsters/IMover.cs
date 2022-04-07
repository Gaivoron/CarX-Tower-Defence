using System;
using UnityEngine;

namespace TowerDefence.Monsters
{
    public interface IMover
    {
        event Action FinishReached;

        float EstimatedTime { get; }
        Vector3 Position { get; }
        //TODO - turn into an extension method of a sort?
        Vector3 PredictPosition(float time);
    }
}