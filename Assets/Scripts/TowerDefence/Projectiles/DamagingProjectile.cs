using UnityEngine;
namespace TowerDefence.Projectiles
{
	public class DamagingProjectile : MonoBehaviour
	{
		//TODO - make private?
		public int m_damage = 10;

		void OnTriggerEnter(Collider other)
		{
			//TODO - check flag on gameobject?
			var monster = other.gameObject.GetComponent<Monster>();
			if (monster == null)
				return;

			monster.HP -= m_damage;
			Destroy(gameObject);
		}
	}
}