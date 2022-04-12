using UnityEngine;
namespace TowerDefence.Projectiles
{
    public sealed class SelfdestroyOnHit : MonoBehaviour
	{
		[SerializeField]
		private string m_targetTag = "Map";

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag(m_targetTag))
			{
				Debug.Log($"{name} selfdestroyed when hit {other.name}");
				Destroy(gameObject);
			}
		}
	}
}