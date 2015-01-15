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
		private MusicManager m_musicManager;

		private bool m_bStateChanged = true;

		void Start () 
		{
			m_gameLogic = GetComponent<GameLogic>();
			m_musicManager = GetComponent<MusicManager>();
		}

		void Update () 
		{
			switch (m_gameState) 
			{
			case State.MAINMENU:

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
				break;
			case State.DEAD:
				break;
			case State.MAP:
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

