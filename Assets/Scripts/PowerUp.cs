using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class PowerUp : MonoBehaviour 
	{
		public enum Type
		{
			COIN,
			COFFEECUP,
			SPEEDPILL,
			SLOWPILL,
			PEASOUP
		}

		public Type powerUpType = Type.COIN;
		public SoundType soundType = SoundType.POWERUP_COIN;
		public GameObject hitEffectPrefab;

		private AudioManager m_audioManager;

		public void Start()
		{
			m_audioManager = GameObject.Find ("SceneEssentials").GetComponent<AudioManager>();
		}

		public PowerUp.Type PowerUpType
		{
			get { return powerUpType; }
		}

		public void Hit()
		{
			m_audioManager.PlaySound(soundType);
			Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}

	}

}
