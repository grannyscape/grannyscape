using UnityEngine;
using System.Collections;

namespace grannyscape
{
	public enum State
	{
		MAINMENU,
		LEVELSTART,
		LEVELRUNNING,
		LEVELEND,
		DEAD,
		MAP,
	}

	public class GameStateController : MonoBehaviour 
	{
		private State m_gameState = State.LEVELSTART;
		
		private GameLogic m_gameLogic;
		private PersistentData m_persistentData;

		private bool m_bStateChanged = true;

		void Start () 
		{
			m_gameLogic = GetComponent<GameLogic>();
			m_persistentData = PersistentData.instance;
		}

		void Update () 
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Application.LoadLevel(0);
			}
			if(Input.GetKeyDown(KeyCode.R))
			{
				Application.LoadLevel(m_persistentData.CurrentLevel);
			}

			switch (m_gameState) 
			{
			case State.MAINMENU:
				//main menu is a separate scene?
				break;
			case State.LEVELSTART:
				if (Input.anyKeyDown)
				{
					GameState = State.LEVELRUNNING;
				}
				break;
			case State.LEVELRUNNING:
				break;
			case State.LEVELEND:
				if(Input.anyKeyDown)
				{
					GameState = State.MAP;
				}
				break;
			case State.DEAD:
				if(Input.anyKeyDown)
				{
					Application.LoadLevel(m_persistentData.CurrentLevel);
				}
				break;
			case State.MAP:
				//Debug.Log ("currentLevel: " + m_persistentData.CurrentLevel);
				if(Input.anyKeyDown)
				{
					if(m_persistentData.CurrentLevel < 5)
					{
						m_persistentData.CurrentLevel++;
						Application.LoadLevel(m_persistentData.CurrentLevel);
					}
					else
					{
						Application.LoadLevel(0);
					}
				}
				break;
			}

			// notify game logic if the game state has changed
			if(m_bStateChanged)
			{
				m_gameLogic.StateChanged();
				m_bStateChanged = false;
			}
		}

		public State GameState
		{
			get { return m_gameState; }
			set 
			{ 
				m_gameState = value; 
				m_bStateChanged = true;
			}
		}

	}

}

