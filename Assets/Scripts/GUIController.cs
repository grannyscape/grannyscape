using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace grannyscape
{
	public enum DeathReason
	{
		OUT_OF_COFFEE,
		KICKET_THE_BUCKET
	}

	public class GUIController : MonoBehaviour 
	{	
		private GameObject m_healthBarSlider; 
		private GameObject m_peasoupSlider;
		private GameObject m_moneyText;

		private GameObject m_gameOverText;
		private GameObject m_pressAnyKey;

		private GameObject[] m_startTexts = new GameObject[5];
		private GameObject[] m_endTexts = new GameObject[5];

		private GameObject[] m_maps = new GameObject[6];
		
		void Start()
		{
			m_healthBarSlider = GameObject.Find ("HealthSlider");
			m_moneyText = GameObject.Find ("MoneyText");

			m_peasoupSlider = GameObject.Find ("PeasoupSlider");

			m_gameOverText = GameObject.Find ("GameOver");
			m_pressAnyKey = GameObject.Find ("PressAnyKey");

			m_gameOverText.SetActive (false);
			m_pressAnyKey.SetActive (false);

			for(int i=1; i < 6; i++)
			{
				string s1 = "level"+i+"_start";
				m_startTexts[i-1] = GameObject.Find(s1);
				m_startTexts[i-1].SetActive(false);

				string s2 = "level"+i+"_end";
				m_endTexts[i-1] = GameObject.Find(s2);
				m_endTexts[i-1].SetActive(false);
			}

			for(int i=0; i<6; i++)
			{
				m_maps[i] = GameObject.Find ("map_0"+i);
				m_maps[i].SetActive(false);
			}
		}

		public void SetHealth(float health)
		{
			m_healthBarSlider.GetComponent<Slider>().value = health;
		}

		public void SetMoney(int money)
		{
			m_moneyText.GetComponent<Text>().text = money.ToString("0000");
		}

		public void SetDead(bool dead, DeathReason reason)
		{
			if(dead)
			{
				if(reason == DeathReason.OUT_OF_COFFEE)
					m_gameOverText.GetComponent<Text>().text = "Run out of coffee!";
				else
					m_gameOverText.GetComponent<Text>().text = "Granny kicket the bucket!";
			}

			m_gameOverText.SetActive(dead);
		}

		public void ShowPressAnyKey(bool visible)
		{
			m_pressAnyKey.SetActive(visible);
		}

		public void LevelFinished(bool finished)
		{
			//m_levelFinishedText.SetActive(finished);
		}
			
		public void SetPeasoup(float peasoup)
		{
			m_peasoupSlider.GetComponent<Slider>().value = peasoup;
		}

		public void ShowStartText(int level, bool visible)
		{
			m_startTexts[level-1].SetActive(visible);
		}

		public void ShowEndText(int level, bool visible)
		{
			m_endTexts[level-1].SetActive(visible);
		}

		public void ShowMap(int level, bool visible)
		{
			m_maps[level].SetActive(visible);
		}
	}

}
