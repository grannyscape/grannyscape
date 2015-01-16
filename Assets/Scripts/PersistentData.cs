using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class PersistentData : MonoBehaviour 
	{
		public static PersistentData instance;

		private float m_health = 1f;	// coffee
		private int m_money = 0;
		private int m_peasoup = 0;
		private int m_currentLevel = 1; //TODO: change when we have main menu

		public int currentLevelDebug = 1;

		void Awake() 
		{
			if(instance == null) 
			{
				m_currentLevel = currentLevelDebug;
				instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(this); // or gameObject
			}
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
