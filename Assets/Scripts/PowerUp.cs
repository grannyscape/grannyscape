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

	
		public PowerUp.Type PowerUpType
		{
			get { return powerUpType; }
		}

		public void Hit()
		{
			Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}

	}

}
