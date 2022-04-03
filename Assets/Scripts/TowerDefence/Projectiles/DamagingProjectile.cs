using TowerDefence.Monsters;
using UnityEngine;
namespace TowerDefence.Projectiles
{
	public class DamagingProjectile : MonoBehaviour
	{
		//TODO - make private?
		public int m_damage = 10;

		private void OnTriggerEnter(Collider other)
		{
			//TODO - check flag on gameobject?
			var monster = other.gameObject.GetComponent<IMonster>();
			if (monster == null)
			{
				return;
			}

			monster.HP -= m_damage;
			Destroy(gameObject);
		}
	}
}