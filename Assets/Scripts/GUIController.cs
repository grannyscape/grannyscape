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
	}

}
