using UnityEngine;
using System.Collections;

namespace grannyscape
{
	public class CameraController : MonoBehaviour 
	{
		public GameObject player;

		private float m_startX;
		private float m_playerStartX;

		void Start () 
		{
			m_startX = transform.position.x;
			m_playerStartX = player.transform.position.x;
		}

		void LateUpdate()
		{
			float playerDeltaX = player.transform.position.x - m_playerStartX;

			Vector3 pos = transform.position;
			pos.x = m_startX + playerDeltaX;
			transform.position = pos;
		}
	}

}