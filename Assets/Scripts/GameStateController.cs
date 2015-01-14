using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class GameStateController : MonoBehaviour 
	{
		enum State
		{
			MainMenu,
			LevelStart,
			LevelRunning,
			LevelEnd,
			EndScreen,
		}

		private State m_gameState = State.LevelStart;

		void Start () 
		{
		
		}

		void Update () 
		{
			switch (m_gameState) 
			{
			case State.MainMenu:
				break;
			case State.LevelStart:
				break;
			case State.LevelRunning:
				break;
			case State.LevelEnd:
				break;
			case State.EndScreen:
				break;
			}
		}
	}

}
