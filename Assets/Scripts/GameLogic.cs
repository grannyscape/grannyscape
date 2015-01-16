using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace grannyscape
{

	public class GameLogic : MonoBehaviour 
	{
		public float reduceHealthPerSecond = 0.005f;
		public float coffeecupHealthBonus = 0.3f;
		public float speedChange = 2f;
		public float speedChangeTime = 2f;
		public float maxPeasoupAmount = 4f;

		private bool m_bFastSpeed = false;
		private bool m_bSlowSpeed = false;

		private float m_elapsedTime = 0f;
		private int m_fastSpeedUpgrades = 0;

		private PlayerController m_playerController;
		private GUIController m_guiController;
		private GameStateController m_gameStateController;
		private MusicManager m_musicManager;
		private AudioManager m_audioManager;

		private PersistentData m_persistentData;

		private GameObject m_ambulance;

		// Use this for initialization
		void Start () 
		{
			m_guiController = GetComponent<GUIController> ();
			m_gameStateController = GetComponent<GameStateController> ();
			m_playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController>();
			m_musicManager = GetComponent<MusicManager>();
			m_audioManager = GetComponent<AudioManager>();

			m_ambulance = GameObject.Find("ambulance");


			m_persistentData = PersistentData.instance;
			m_persistentData.Coffee = 1f;
		}

		// Update is called once per frame
		void Update () 
		{
			m_elapsedTime = m_elapsedTime + Time.deltaTime;

			// die if coffee runs out
			if(m_persistentData.Coffee <= 0 && m_gameStateController.GameState == State.LEVELRUNNING)
			{
				SetDead(DeathReason.OUT_OF_COFFEE);
			}

			// stop stimulants if effects vanish
			if (m_bFastSpeed && m_elapsedTime >= speedChangeTime) 
			{
				StopFastSpeed();
			}

			if (m_bSlowSpeed && m_elapsedTime >= speedChangeTime) 
			{
				StopSlowSpeed();
			}
		}

		public void StateChanged()
		{
			Music music = Music.NONE;

			switch(m_gameStateController.GameState)
			{
			case State.LEVELSTART:
				m_guiController.ShowStartText(m_persistentData.CurrentLevel, true);
				music = Music.GAME;
				break;
			case State.LEVELRUNNING:
				m_guiController.ShowStartText(m_persistentData.CurrentLevel, false);
				InvokeRepeating("ReduceHealth", 0.0f, 1f);
				break;
			case State.DEAD:
				CancelInvoke ("ReduceHealth");
				break;
			case State.LEVELEND:
				music = Music.WIN;
				m_playerController.StopMovement();
				m_guiController.ShowEndText(m_persistentData.CurrentLevel, true);
				m_audioManager.PlaySound(SoundType.GRANNY_WIN);
				CancelInvoke ("ReduceHealth");
				break;
			case State.MAP:
				m_guiController.ShowMap(m_persistentData.CurrentLevel, true);
				music = Music.MENU;
				break;
			}

			if(music != Music.NONE)
			{
				m_musicManager.PlayMusic(music);
			}
		}

		public void PowerUpCollected(PowerUp.Type type)
		{
			switch(type)
			{
			case PowerUp.Type.COIN:
				m_persistentData.Money++; 
				m_guiController.SetMoney(m_persistentData.Money);
				break;
			case PowerUp.Type.COFFEECUP:
				m_persistentData.Coffee = m_persistentData.Coffee  + coffeecupHealthBonus;
				Mathf.Clamp(m_persistentData.Coffee, 0f, 1f);
				m_guiController.SetHealth(m_persistentData.Coffee );
				break;
			case PowerUp.Type.SPEEDPILL:
				m_playerController.ChangeMaxSpeed(speedChange);
				m_bFastSpeed = true;
				m_fastSpeedUpgrades++;
				m_elapsedTime = 0f;
				break;
			case PowerUp.Type.SLOWPILL:
				m_audioManager.SetPitch(0.5f);
				m_musicManager.SetPitch(0.5f);
				StopFastSpeed();
				m_bSlowSpeed = true;
				Time.timeScale = 0.5f;
				m_elapsedTime = 0f;
				break;
			case PowerUp.Type.PEASOUP:
				AddPeasoup();
				break;
			}
		}

		public void SetDead(DeathReason reason)
		{
			m_audioManager.PlaySound(SoundType.GRANNY_LOSE);
			StopSlowSpeed();
			
			m_gameStateController.GameState = State.DEAD;
			m_guiController.SetDead (true, reason);

			// drop granny out of the game area
			if(m_playerController.transform.position.y > -2)
			{
				Vector3 pos = m_playerController.transform.position;
				pos.y = -3;

				iTween.RotateTo(m_playerController.gameObject, new Vector3(0f, 0f, 180f), 6f);
				iTween.MoveTo (m_playerController.gameObject, iTween.Hash(
					"position", pos,
					"time", 2f,
					"oncompletetarget", gameObject,
					"oncomplete", "DeathAnimDone"
					));
			}
			else
			{
				DeathAnimDone();
			}
		}

		void DeathAnimDone()
		{
			//quick fix to prevent granny falling indefinitely
			m_playerController.gameObject.rigidbody2D.isKinematic = true; 

			Debug.Log("Granny kicket the bucket!");
			m_audioManager.PlaySound(SoundType.AMBULANCE);
			Transform ambulanceEnd = GameObject.Find("ambulanceEnd").transform;
			iTween.MoveTo(m_ambulance, iTween.Hash(
				"position", ambulanceEnd,
				"time", 2f,
				"easetype", iTween.EaseType.linear,
				"oncompletetarget", gameObject,
				"oncomplete", "WaitForKeyPress"
				));
		}

		void WaitForKeyPress()
		{
			m_guiController.SetDead(false, DeathReason.KICKET_THE_BUCKET);
			m_guiController.ShowPressAnyKey(true);
		}

		public void LevelFinished()
		{
			m_gameStateController.GameState = State.LEVELEND;
		}
	
		void ReduceHealth()
		{
			m_persistentData.Coffee = m_persistentData.Coffee - reduceHealthPerSecond;
			m_guiController.SetHealth(m_persistentData.Coffee);
		}

		void StopFastSpeed()
		{
			for(int i=0; i<m_fastSpeedUpgrades; i++)
			{
				m_playerController.ChangeMaxSpeed(-speedChange);
			}
			m_fastSpeedUpgrades = 0;
			m_bFastSpeed = false;
		}

		void StopSlowSpeed()
		{
			m_audioManager.SetPitch(1f);
			m_musicManager.SetPitch(1f);
			Time.timeScale = 1f;
			m_bSlowSpeed = false;
		}

		void AddPeasoup()
		{
			if (m_persistentData.Peasoup < maxPeasoupAmount) 
			{
				m_persistentData.Peasoup = m_persistentData.Peasoup + 1;
			}

			m_guiController.SetPeasoup((float)m_persistentData.Peasoup / maxPeasoupAmount);
		}

		public void RemovePeasoup()
		{
			if (m_persistentData.Peasoup  > 0) 
			{
				m_persistentData.Peasoup  = m_persistentData.Peasoup  - 1;
			}
			m_guiController.SetPeasoup ((float)m_persistentData.Peasoup  / maxPeasoupAmount);
		}
	}

}
