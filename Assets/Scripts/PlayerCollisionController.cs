using UnityEngine;
using System.Collections;

namespace grannyscape
{	
	public class PlayerCollisionController : MonoBehaviour 
	{
		private GameLogic m_gameLogic;

		void Start()
		{
			m_gameLogic = GameObject.Find ("SceneEssentials").GetComponent<GameLogic>();
		
		}

		void OnTriggerEnter2D(Collider2D coll)
		{
			switch(coll.tag)
			{
			case "Powerup":
				PowerUp p = coll.gameObject.GetComponent<PowerUp>();
				m_gameLogic.PowerUpCollected(p.PowerUpType);
				coll.gameObject.GetComponent<PowerUp>().Hit();
				break;
			case "Finish":
				m_gameLogic.LevelFinished();
				break;
			default:
				break;
			}

		}

	}

}