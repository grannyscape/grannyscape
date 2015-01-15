using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace grannyscape
{

	public class GUIController : MonoBehaviour 
	{
		private GameObject m_player;
		
		private GameObject m_healthBarSlider; 
		private GameObject m_moneyText;

		private GameObject m_gameOverText; 
		private GameObject m_levelFinishedText;

		void Start()
		{
			m_player = GameObject.FindGameObjectWithTag("Player");

			m_healthBarSlider = GameObject.Find ("HealthSlider");
			m_moneyText = GameObject.Find ("MoneyText");
			m_gameOverText = GameObject.Find ("GameOver");
			m_levelFinishedText = GameObject.Find ("LevelComplete");

			m_gameOverText.SetActive (false);
			m_levelFinishedText.SetActive (false);
		}

		public void SetHealth(float health)
		{
			m_healthBarSlider.GetComponent<Slider>().value = health;
		}

		public void SetMoney(int money)
		{
			//m_moneyTexttext = "Money: " + money;
		}

		public void SetDead(bool dead)
		{
			m_gameOverText.SetActive(dead);
		}

		public void LevelFinished(bool finished)
		{
			m_levelFinishedText.SetActive(finished);
		}
			
	}

}
