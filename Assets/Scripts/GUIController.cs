using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace grannyscape
{

	public class GUIController : MonoBehaviour 
	{
		public GameObject player;
		
		public Slider healthBarSlider; 
		public Text moneyText;

		public Text gameOverText; 
		public Text levelFinishedText;
		
		void Start()
		{
			
		}
		
		// Update is called once per frame
		void Update () 
		{

		}

		public void SetHealth(float health)
		{
			healthBarSlider.value = health;
		}

		public void SetMoney(int money)
		{
			moneyText.text = "Money: " + money;
		}

		public void SetDead(bool dead)
		{
			gameOverText.gameObject.SetActive(dead);
		}

		public void LevelFinished(bool finished)
		{
			levelFinishedText.gameObject.SetActive(finished);
		}
			
	}

}
