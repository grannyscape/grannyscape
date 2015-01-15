using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace grannyscape
{

	public class GameLogic : MonoBehaviour 
	{
		public float reduceHealthPerSecond = 0.005f;
		public float coffeecupHealthBonus = 0.3f;

		private float m_health = 1f;
		private int m_money = 0;

		private bool m_bDead = false;

		private GUIController m_guiController;
		private GameStateController m_gameStateController;


		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		// Use this for initialization
		void Start () 
		{
			m_guiController = GetComponent<GUIController>();
			m_gameStateController = GetComponent<GameStateController>();

			Reset();
		}

		void Reset()
		{
			m_guiController.SetDead(false);
			m_guiController.LevelFinished(false);
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(m_health <= 0 && m_gameStateController.GameState == State.LEVELRUNNING)
			{
				m_bDead = true;
				m_gameStateController.GameState = State.DEAD;
				m_guiController.SetDead(true);
			}
		}

		public void StateChanged()
		{
			if(m_gameStateController.GameState == State.LEVELRUNNING)
			{
				InvokeRepeating("ReduceHealth", 0.0f, 1f);
			}
			else
			{
				CancelInvoke ("ReduceHealth");
			}
		}

		public void PowerUpCollected(PowerUp.Type type)
		{
			switch(type)
			{
			case PowerUp.Type.COIN:
				m_money++;
				m_guiController.SetMoney(m_money);
				break;
			case PowerUp.Type.COFFEECUP:
				m_health = m_health + coffeecupHealthBonus;
				Mathf.Clamp(m_health, 0f, 1f);
				m_guiController.SetHealth(m_health);
				break;
			case PowerUp.Type.SPEEDPILL:
				break;
			case PowerUp.Type.SLOWPILL:
				break;
			case PowerUp.Type.PEASOUP:
				break;
			}
		}

		public void LevelFinished()
		{
			m_gameStateController.GameState = State.LEVELEND;
			m_guiController.LevelFinished(true);
		}
	
		void ReduceHealth()
		{
			m_health = m_health - reduceHealthPerSecond;
			m_guiController.SetHealth(m_health);
		}
	}

}
