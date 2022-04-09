using TowerDefence.Monsters;
using UnityEngine;
namespace TowerDefence.Projectiles
{
    public sealed class DamageOnHit : MonoBehaviour
	{
		[SerializeField]
		private string m_targetTag = "Monster";
		[SerializeField]
		private int m_damage = 10;

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.CompareTag(m_targetTag))
			{
				return;
			}

			var monster = other.gameObject.GetComponent<IMonster>();
			if (monster == null)
			{
				Debug.LogWarning($"No component implementing {nameof(IMonster)} on {other.name}!");
				return;
			}

			monster.HP -= m_damage;
			Destroy(gameObject);
		}
	}
}