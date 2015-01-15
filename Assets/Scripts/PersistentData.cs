using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class PersistentData : MonoBehaviour 
	{
		private float m_health = 1f;	// coffee
		private int m_money = 0;
		private int m_peasoup = 0;
		private int m_currentLevel = 1;

		void Awake() 
		{
			DontDestroyOnLoad(gameObject);
		}
		
		public float Coffee
		{
			get { return m_health; }
			set { m_health = value; }
		}

		public int Money
		{
			get { return m_money; }
			set { m_money = value; }
		}

		public int Peasoup 
		{
			get { return m_peasoup; }
			set { m_peasoup = value; }
		}

		public int CurrentLevel
		{
			get { return m_currentLevel; }
			set { m_currentLevel = value; }
		}
	}
}
