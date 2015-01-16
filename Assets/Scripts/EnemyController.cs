using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class EnemyController : MonoBehaviour 
	{
		public GameObject hitEffectPrefab;

		private AudioManager m_audioManager;
		
		public void Start()
		{
			m_audioManager = GameObject.Find ("SceneEssentials").GetComponent<AudioManager>();
		}

		public void Hit()
		{
			m_audioManager.PlaySound(SoundType.PUNK_HIT);
			Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

}
