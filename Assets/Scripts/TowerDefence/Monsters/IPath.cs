using UnityEngine;

namespace TowerDefence.Monsters
{
    public interface IPath
    {
        float Length { get; }

        Vector3 GetPosition(float progress);
    }
}