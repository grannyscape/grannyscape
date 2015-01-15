using UnityEngine;
using System.Collections;

namespace grannyscape
{
	public class CameraController : MonoBehaviour 
	{
		private GameObject m_player;

		private float m_startX;
		private float m_playerStartX;

		void Start () 
		{
			m_player = GameObject.FindGameObjectWithTag("Player");
			m_startX = transform.position.x;
			m_playerStartX = m_player.transform.position.x;
		}

		void LateUpdate()
		{
			float playerDeltaX = m_player.transform.position.x - m_playerStartX;

			Vector3 pos = transform.position;
			pos.x = m_startX + playerDeltaX;
			transform.position = pos;
		}
	}

}