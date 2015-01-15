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
		public GameObject hitEffectPrefab;
		public AudioClip[] soundEffects;


		public PowerUp.Type PowerUpType
		{
			get { return powerUpType; }
		}

		public void Hit()
		{
			if (soundEffects.Length > 0) 
			{
				AudioSource.PlayClipAtPoint (soundEffects [Random.Range (0, soundEffects.Length - 1)], Camera.main.transform.position);
			}
			Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}

	}

}
