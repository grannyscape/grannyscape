using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace grannyscape
{

	public class GameLogic : MonoBehaviour 
	{
		public float reduceHealthPerSecond = 0.005f;
		public float coffeecupHealthBonus = 0.3f;
		public float speedIncrease = 2f;
		public float speedIncreaseTime = 2f;

		private float m_health = 1f;
		private int m_money = 0;

		//private bool m_bDead = false;
		private bool m_bFastSpeed = false;
		private bool m_bSlowSpeed = false;

		private float m_elapsedTime = 0f;
		private float m_speedChangeStartTime = 0f;
		private int m_fastSpeedUpgrades = 0;

		private PlayerController m_playerController;
		private GUIController m_guiController;
		private GameStateController m_gameStateController;


		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		// Use this for initialization
		void Start () 
		{
			m_guiController = GetComponent<GUIController> ();
			m_gameStateController = GetComponent<GameStateController> ();
			m_playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController>();
		}

		void Reset()
		{
			m_guiController.SetDead(false);
			m_guiController.LevelFinished(false);
		}
		
		// Update is called once per frame
		void Update () 
		{
			m_elapsedTime = m_elapsedTime + Time.deltaTime;

			if(m_health <= 0 && m_gameStateController.GameState == State.LEVELRUNNING)
			{
				SetDead();
			}

			if (m_bFastSpeed && m_elapsedTime >= speedIncreaseTime) 
			{
				StopFastSpeed();
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
				m_playerController.ChangeMaxSpeed(speedIncrease);
				m_bFastSpeed = true;
				m_fastSpeedUpgrades++;
				m_elapsedTime = 0f;
				break;
			case PowerUp.Type.SLOWPILL:
				StopFastSpeed();
				m_elapsedTime = 0f;
				break;
			case PowerUp.Type.PEASOUP:
				break;
			}
		}

		public void SetDead()
		{
			//m_bDead = true;
			m_gameStateController.GameState = State.DEAD;
			m_guiController.SetDead (true);
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

		void StopFastSpeed()
		{
			for(int i=0; i<m_fastSpeedUpgrades; i++)
			{
				m_playerController.ChangeMaxSpeed(-speedIncrease);
			}
			m_fastSpeedUpgrades = 0;
			m_bFastSpeed = false;
			
			Debug.Log ("Fast pill ended");
		}
	}

}
