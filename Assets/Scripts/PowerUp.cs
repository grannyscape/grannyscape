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

		// Use this for initialization
		void Start () 
		{
		
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}

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
